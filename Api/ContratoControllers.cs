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
	public class ContratoController : ControllerBase
	{

        private readonly DataContext Contexto;

		public ContratoController(DataContext dataContext)
		{
			this.Contexto = dataContext;
		}


                // GET: api/<controller>
		[HttpGet]
		public async Task<ActionResult<Contrato>> Get()
		{
			try
			{
				//Funciona retorna los contratos vigentes de los inmuebles del propietario logueado
                var propietario = await Contexto.Propietario.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
                var contrato = Contexto.Contrato.Include(c=> c.inmueble).Include(i=>i.inquilino).Where(c => c.inmueble.dueño.Email == propietario.Email && c.FechaInicio <= DateTime.Now && c.FechaFinal >= DateTime.Now);
                return Ok(contrato);

			}
			catch (Exception ex)
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
				var contrato = await Contexto.Contrato.Include(c=> c.inmueble).Include(i=>i.inquilino).Where(c => c.inmueble.dueño.Email == propietario.Email).ToListAsync();
                return Ok(contrato);
			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		
    }
}