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

		public InmuebleController(DataContext dataContext)
		{
			this.Contexto = dataContext;
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

    }
}