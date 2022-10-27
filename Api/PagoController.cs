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
	public class PagoController : ControllerBase
	{

        private readonly DataContext Contexto;

		public PagoController(DataContext dataContext)
		{
			this.Contexto = dataContext;
		}


                // GET: api/<controller>
		[HttpPost("ObtenerPagos")]
		public async Task<ActionResult<Pago>> ObtenerPagos([FromForm]int id)
		{
			try
			{
				//Funciona retorna los Pagos vigentes de los inmuebles del propietario logueado
                var contrato = await Contexto.Contrato.Include(c=> c.inmueble).Include(i=>i.inquilino).Where(c => c.inmueble.dueño.Email == User.Identity.Name).SingleAsync(c => c.IdContrato == id);
                var pago = Contexto.Pago.Where(x => x.IdContrato == contrato.IdContrato);
                return Ok(pago);

			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
    }
}