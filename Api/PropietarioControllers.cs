using Microsoft.AspNetCore.Mvc;
using Inmobiliaria_2022.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

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

    }
}