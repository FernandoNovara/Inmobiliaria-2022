using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.Models;

namespace Net.Controllers
{
    public class ContratoController : Controller
    {
        RepositorioContrato repositorioContrato;
        RepositorioInmueble repositorioInmueble;
        RepositorioInquilino repositorioInquilino;
        public ContratoController()
        {
            repositorioContrato = new RepositorioContrato();
            repositorioInmueble = new RepositorioInmueble();
            repositorioInquilino = new RepositorioInquilino();
        }

        // GET: Contrato
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.VistaContrato = false;
            var lista = repositorioContrato.ObtenerContratos();
            return View(lista);
        }

        // GET: Contrato/Details/5
        [Authorize]
        public ActionResult Detalles(int id)
        {
            
            var lista = repositorioContrato.ObtenerContrato(id);
            return View(lista);
        }

        // GET: Contrato/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.inmueble = repositorioInmueble.ObtenerInmuebles();
            ViewBag.inquilino = repositorioInquilino.ObtenerInquilinos();
            return View();
        }

        // POST: Contrato/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato contrato)
        {
            try
            {
                int res = repositorioContrato.Alta(contrato);
                if(res > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View();
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        // GET: Contrato/Edit/5
        [Authorize]
        public ActionResult Editar(int id)
        {
            ViewBag.inmueble = repositorioInmueble.ObtenerInmuebles();
            ViewBag.inquilino = repositorioInquilino.ObtenerInquilinos();
            var dato = repositorioContrato.ObtenerContrato(id);
            return View(dato);
        }

        // POST: Contrato/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, IFormCollection collection)
        {
            try
            {
                Contrato c = repositorioContrato.ObtenerContrato(id);
                c.IdInmueble = Int32.Parse(collection["IdInmueble"]);
                c.IdInquilino = Int32.Parse(collection["IdInquilino"]);
                c.FechaInicio = DateTime.Parse(collection["FechaInicio"]);
                c.FechaFinal = DateTime.Parse(collection["FechaFinal"]);
                c.Monto = double.Parse(collection["Monto"]);
                repositorioContrato.Actualizar(c);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        // GET: Contrato/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Eliminar(int id)
        {
            var dato = repositorioContrato.ObtenerContrato(id);
            return View(dato);
        }

        // POST: Contrato/Delete/5
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id, IFormCollection collection)
        {
            try
            {
                repositorioContrato.Baja(id);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        // GET: Contrato
        [Authorize]
        public ActionResult ContratoVigentes()
        {
            ViewBag.VistaContrato = false;
            var lista = repositorioContrato.ObtenerContratosVigentes();
            return View("Index",lista);
        }

        [Authorize]
        public ActionResult ContratoNoVigentes()
        {
            ViewBag.VistaContrato = false;
            var lista = repositorioContrato.ObtenerContratosNoVigentes();
            return View("Index",lista);
        }

        [Authorize]
        public ActionResult ListarContratosPorInmuebles(int id)
        {
            ViewBag.VistaContrato = true;
            var lista = repositorioContrato.ObtenerContratosPorInmueble(id);
            return View("Index",lista);
        }
    }
}