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
	public class InmuebleController : ControllerBase
	{

		private readonly DataContext Contexto;
		private readonly IWebHostEnvironment environment;

		public InmuebleController(DataContext dataContext,IWebHostEnvironment environment)
		{
			this.Contexto = dataContext;
			this.environment = environment;
		}


        // GET: api/<controller>
		[HttpGet]
		public async Task<ActionResult<Inmueble>> Get()
		{
            //funciona (trae una lista de inmuebles del propietario)
			try
			{
                var propietario = await Contexto.Propietario.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
                var inmuebles = Contexto.Inmueble.Where(x => x.IdPropietario == propietario.IdPropietario);
                return Ok(inmuebles);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}


		// GET: api/<controller>/5
        [HttpGet("{id}")]
		public async Task<ActionResult> ObtenerInmueblesPorId(int id)
		{
			try
			{
				var propietario = await Contexto.Propietario.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
                var inmueble = Contexto.Inmueble.Include(x => x.dueño).Where(y => y.dueño.Email == propietario.Email).SingleOrDefault(z => z.IdInmueble == id);
                return inmueble != null ? Ok(inmueble) : NotFound();

				
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("AltaInmueble")]
		public async Task<IActionResult> AltaInmueble([FromBody]Inmueble inmueble)
		{
			try
			{
				if(ModelState.IsValid)
				{

					var propietario = await Contexto.Propietario.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
					inmueble.IdPropietario = propietario.IdPropietario;

					if(inmueble.Imagen != null)
					{
						MemoryStream ms = new MemoryStream(Convert.FromBase64String(inmueble.Imagen));
						IFormFile Imagen = new FormFile(ms,0,ms.Length,"inmueble",".jpg");
						String wwwPath = environment.WebRootPath;
						String path = Path.Combine(wwwPath,"Uploads");
						if(!Directory.Exists(path))
						{
							Directory.CreateDirectory(path);
						}
						
						Random r = new Random();
						String fileName = "inmueble_" + inmueble.IdPropietario + r.Next(0,100000) + Path.GetExtension(Imagen.FileName);
						String pathCompleto = Path.Combine(path,fileName);

						inmueble.Imagen = Path.Combine("http://192.168.1.108:5000/","Uploads/",fileName);
						using (FileStream fs = new FileStream(pathCompleto, FileMode.Create))
						{
							Imagen.CopyTo(fs);
						}
					}
					await Contexto.Inmueble.AddAsync(inmueble);
					Contexto.SaveChanges();
					return CreatedAtAction(nameof(Get),new {id = inmueble.IdInmueble},inmueble);
				}
				else
				{
					return BadRequest();
				}
			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
			

		}

		// GET: api/<controller>/5
		[HttpPost("CambiarEstado")]
		public async Task<ActionResult> CambiarEstado([FromForm]int id)
			{	
				try
				{
					if(ModelState.IsValid)
					{
						var propietario = await Contexto.Propietario.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
						var inmueble = Contexto.Inmueble.Include(x => x.dueño).Where(y => y.dueño.Email == propietario.Email).SingleOrDefault(z => z.IdInmueble == id);
						inmueble.Estado = !inmueble.Estado;
						Contexto.Inmueble.Update(inmueble);
						await Contexto.SaveChangesAsync();
						return Ok(inmueble.Estado);
					}
					else
					{
						return BadRequest();
					}
				}
				catch(Exception ex)
				{
					return BadRequest(ex.Message);
				}
			}

			// GET: api/<controller>/5
		[HttpPost("InmueblesConContrato")]
		public async Task<ActionResult> InmueblesConContrato()
		{	
			try
			{
                var propietario = await Contexto.Propietario.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
				var inmuebles = Contexto.Inmueble.Where(x => x.IdPropietario == propietario.IdPropietario);
				var contrato = Contexto.Contrato.Include(c=> c.inmueble).Include(i=>i.inquilino).Where(c => c.inmueble.dueño.Email == propietario.Email);
                
                return Ok(inmuebles);
			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}



    }
}