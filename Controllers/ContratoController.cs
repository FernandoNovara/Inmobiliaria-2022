using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public ActionResult Index()
        {
            var lista = repositorioContrato.ObtenerContratos();
            return View(lista);
        }

        // GET: Contrato/Details/5
        public ActionResult Detalles(int id)
        {
            
            var lista = repositorioContrato.ObtenerContrato(id);
            return View(lista);
        }

        // GET: Contrato/Create
        public ActionResult Create()
        {
            ViewBag.inmueble = repositorioInmueble.ObtenerInmuebles();
            ViewBag.inquilino = repositorioInquilino.ObtenerInquilinos();
            return View();
        }

        // POST: Contrato/Create
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
        public ActionResult Editar(int id)
        {
            ViewBag.inmueble = repositorioInmueble.ObtenerInmuebles();
            ViewBag.inquilino = repositorioInquilino.ObtenerInquilinos();
            var dato = repositorioContrato.ObtenerContrato(id);
            return View(dato);
        }

        // POST: Contrato/Edit/5
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
        public ActionResult Eliminar(int id)
        {
            var dato = repositorioContrato.ObtenerContrato(id);
            return View(dato);
        }

        // POST: Contrato/Delete/5
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
    }
}