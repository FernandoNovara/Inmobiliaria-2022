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
	public class InquilinoController : ControllerBase
	{

        private readonly DataContext Contexto;

		public InquilinoController(DataContext dataContext)
		{
			this.Contexto = dataContext;
		}


		// GET: api/<controller>
		[HttpGet("{id}")]
		public async Task<ActionResult> Get(int id)
		{
			try
			{
				//Esta funcionando y retorna el inquilino del contrato segun el id del contrato
				var propietario = await Contexto.Propietario.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
                var contrato = Contexto.Contrato.Include(c=> c.inmueble).Include(i=>i.inquilino).Where(c => c.inmueble.dueño.Email == propietario.Email).SingleOrDefault(c => c.IdContrato == id);
                return Ok(contrato.inquilino);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
    }
}