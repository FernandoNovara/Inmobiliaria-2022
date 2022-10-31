using Microsoft.AspNetCore.Mvc;
using Inmobiliaria_2022.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit;
using Microsoft.AspNetCore.Http.Features;

namespace Inmobiliaria_2022.Api
{
	[Route("api/[controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[ApiController]
	public class PropietarioController : ControllerBase
	{

		private readonly DataContext Contexto;
        private readonly IConfiguration config;
		private readonly IWebHostEnvironment environment;

		public PropietarioController(DataContext dataContext, IConfiguration config,IWebHostEnvironment environment)
		{
			this.Contexto = dataContext;
            this.config = config;
			this.environment = environment;
		}

        // GET: api/<controller>
		[HttpGet]
		public async Task<ActionResult<Propietario>> Get()
		{
			try
			{
				// Funciona retorna los datos del usuario logueado
                var propietario = await Contexto.Propietario.FirstOrDefaultAsync(x => x.Email == User.Identity.Name );
                return propietario != null ? Ok(propietario) : NotFound();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        // POST api/<controller>/login
		[HttpPost("login")]
		[AllowAnonymous]
		public async Task<IActionResult> Login([FromBody] LoginView loginView)
		{
			try
			{
				string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
				password: loginView.Clave,
				salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
				prf: KeyDerivationPrf.HMACSHA1,
				iterationCount: 1000,
				numBytesRequested: 256 / 8));
				var p = await Contexto.Propietario.FirstOrDefaultAsync(x => x.Email == loginView.Usuario);
				if (p == null || p.Clave != hashed)
				{
					return BadRequest("Nombre de usuario o clave incorrecta");
				}
				else
				{
					var key = new SymmetricSecurityKey(
						System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
					var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, p.Email),
						new Claim("FullName", p.Nombre + " " + p.Apellido),
						new Claim(ClaimTypes.Role, "Propietarios"),
					};

					var token = new JwtSecurityToken(
						issuer: config["TokenAuthentication:Issuer"],
						audience: config["TokenAuthentication:Audience"],
						claims: claims,
						expires: DateTime.Now.AddMinutes(60),
						signingCredentials: credenciales
					);
					return Ok(new JwtSecurityTokenHandler().WriteToken(token));
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// Post: api/<controller>
		[HttpPost("actualizar")]
		public async Task<IActionResult> Actualizar([FromBody] Propietario usuario)
		{
			
			try
			{
				if(ModelState.IsValid)
				{
					Propietario original = await Contexto.Propietario.AsNoTracking().SingleAsync(x => x.Email == User.Identity.Name);
					usuario.IdPropietario = original.IdPropietario;
					if(String.IsNullOrEmpty(usuario.Clave))
					{
						usuario.Clave = original.Clave;
						
					}
					else
					{
						string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
						password: usuario.Clave,
						salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
						prf: KeyDerivationPrf.HMACSHA1,
						iterationCount: 1000,
						numBytesRequested: 256 / 8));
						usuario.Clave = hashed;
					}
					Contexto.Propietario.Update(usuario);
					await Contexto.SaveChangesAsync();
					return Ok(usuario);
				}
				else
				{
					return BadRequest();
				}   
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("token")]
		public async Task<IActionResult> token()
		{
			try
			{
				var perfil = new 
				{
					Email = User.Identity.Name,
					Nombre = User.Claims.First(x => x.Type == "FullName").Value,
					Rol = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value
				};

				Random rand = new Random(Environment.TickCount);
                string randomChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";
                string ClaveNueva = "";
                for (int i = 0; i < 8; i++)
                {
                    ClaveNueva += randomChars[rand.Next(0, randomChars.Length)];
                }
				var sinHash = ClaveNueva;
                ClaveNueva = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: ClaveNueva,
                            salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                            prf: KeyDerivationPrf.HMACSHA1,
                            iterationCount: 1000,
                            numBytesRequested: 256 / 8));


                Propietario original = await Contexto.Propietario.AsNoTracking().FirstOrDefaultAsync(x => x.Email == perfil.Email);
                original.Clave = ClaveNueva;
                Contexto.Propietario.Update(original);
                await Contexto.SaveChangesAsync();

				var message = new MimeKit.MimeMessage();
				message.To.Add(new MailboxAddress(perfil.Nombre,perfil.Email));
				message.From.Add(new MailboxAddress("Inmobiliaria La Toma",perfil.Email));
				message.Subject = "Prueba de Correo desde api";
				message.Body = new TextPart("html")
				{
					Text = @$"<h1>Restablecimiento de contraseña</h1>
							<p>¡Bienvenido, {perfil.Nombre}!<br>
							Hemos restablecido tu contraseña, Ahora podras acceder a tu cuenta con la siguiente contraseña: {sinHash}<br></p>
							<p><span style='color: #f00;    font-size: 20px; font-weight: bold;'>Atencion</span> :Recuerda que tendrás acceso solo por 15 min antes que deje ser valida.</p>",
				};
				message.Headers.Add("Encabezado","Valor");
				MailKit.Net.Smtp.SmtpClient client = new SmtpClient();
				client.ServerCertificateValidationCallback = (object sender,
					System.Security.Cryptography.X509Certificates.X509Certificate certificate,
					System.Security.Cryptography.X509Certificates.X509Chain chain,
					System.Net.Security.SslPolicyErrors sslPolicyErrors) => {return true;};
				client.Connect("smtp.gmail.com",465,MailKit.Security.SecureSocketOptions.Auto);
				client.Authenticate(config["SMTPUser"],config["SMTPPass"]);
				// client.Authenticate("fernandoarielnovara@gmail.com","nbzsbyfmavxjahga");
				await client.SendAsync(message);
				return Ok(perfil);
			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("ObtenerEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerEmail([FromForm] string email)
        {
            try
            {
                var feature = HttpContext.Features.Get<IHttpConnectionFeature>();
                var LocalPort = feature?.LocalPort.ToString();
                var ipv4 = HttpContext.Connection.LocalIpAddress.MapToIPv4().ToString();
                var ipConexion = "http://" + ipv4 + ":" + LocalPort + "/";

                var entidad = await Contexto.Propietario.FirstOrDefaultAsync(x => x.Email == email);
                var key = new SymmetricSecurityKey(
                        System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
                var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, entidad.Email),
                        new Claim("FullName", entidad.Nombre + " " + entidad.Apellido),
                        new Claim("IdPropietario", entidad.IdPropietario + " " ),
                        new Claim(ClaimTypes.Role, "Propietario"),

                    };

                var token = new JwtSecurityToken(
                    issuer: config["TokenAuthentication:Issuer"],
                    audience: config["TokenAuthentication:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: credenciales
                );
                var access_token = new JwtSecurityTokenHandler().WriteToken(token);

                var direccion = ipConexion + "Api/Propietario/token?access_token=" + access_token;
                try
                {
					var message = new MimeKit.MimeMessage();
					message.To.Add(new MailboxAddress(entidad.Nombre,entidad.Email));
					message.From.Add(new MailboxAddress("Inmobiliaria La Toma",entidad.Email));
					message.Subject = "Verificacion de identidad";
					message.Body = new TextPart("html")
					{
						Text = @$"<h1>Hola {entidad.Nombre}!</h1>
						<p>Si usted solicito el cambio de contraseña,Ingrese al siguiente link para restablecela:<a href={direccion} >Restablecer Contraseña</a> </p><br>
						<p>Si tú no la solicitaste, desestimá este e-mail.</p>",
					};
                    message.Headers.Add("Encabezado", "Valor");
                    MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient();
                    client.ServerCertificateValidationCallback = (object sender,
                    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                    System.Net.Security.SslPolicyErrors sslPolicyErrors) =>
                    { return true; };
                    client.Connect("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.Auto);
                    client.Authenticate(config["SMTPUser"], config["SMTPPass"]);
                    await client.SendAsync(message);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                return Ok(entidad);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


		[HttpPost("ResetearPass")]
		public async Task<IActionResult> ResetearPass([FromForm] String contraseña)
		{
			try
			{
                var propietario = await Contexto.Propietario.FirstOrDefaultAsync(x => x.Email == User.Identity.Name );
				if(!String.IsNullOrEmpty(contraseña))
				{
					var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: contraseña,
                            salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                            prf: KeyDerivationPrf.HMACSHA1,
                            iterationCount: 1000,
                            numBytesRequested: 256 / 8));
					propietario.Clave = hashed;
				}
                Contexto.Propietario.Update(propietario);
                await Contexto.SaveChangesAsync();
				return Ok(propietario);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

    }
}