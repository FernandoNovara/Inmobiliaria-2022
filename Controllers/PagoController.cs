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
    public class PagoController : Controller
    {

        private RepositorioPago repositorioPago;
        private RepositorioContrato repositorioContrato;

        public PagoController()
        {
            repositorioPago = new RepositorioPago();
            repositorioContrato = new RepositorioContrato();
        }

        // GET: Pago
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.VistaPago = false;
            var lista = repositorioPago.ObtenerPagos();
            ViewBag.contrato = repositorioContrato.ObtenerContratos();
            return View(lista);
        }

        // GET: Pago/Details/5
        [Authorize]
        public ActionResult Detalles(int id)
        {
            var lista = repositorioPago.ObtenerPago(id);
            ViewBag.contrato = repositorioContrato.ObtenerContrato(lista.contrato.IdContrato);
            return View(lista);
        }

        // GET: Pago/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.contrato = repositorioContrato.ObtenerContratos();
            return View();
        }

        // POST: Pago/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pago p)
        {
            try
            {
                var res = repositorioPago.Alta(p);
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
                throw(ex);
            }
        }

        // GET: Pago/Edit/5
        [Authorize]
        public ActionResult Editar(int id)
        {
            var lista = repositorioPago.ObtenerPago(id);
            ViewBag.contrato = repositorioContrato.ObtenerContratos();
            return View(lista);
        }

        // POST: Pago/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, IFormCollection collection)
        {
            try
            {
                Pago p = repositorioPago.ObtenerPago(id);
                p.IdContrato = Int32.Parse(collection["IdContrato"]);
                p.FechaEmision = DateTime.Parse(collection["FechaEmision"]);
                p.Importe = Double.Parse(collection["Importe"]);
                repositorioPago.Actualizar(p);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Pago/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Eliminar(int id)
        {
            var lista = repositorioPago.ObtenerPago(id);
            ViewBag.contrato = repositorioContrato.ObtenerContrato(lista.contrato.IdContrato);
            return View(lista);
        }

        // POST: Pago/Delete/5
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id, IFormCollection collection)
        {
            try
            {
                repositorioPago.Baja(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        // GET: Pago/Delete/5
        [Authorize]
        public ActionResult ListarPagosPorContratos(int id)
        {
            ViewBag.VistaPago = true;
            ViewBag.contrato = repositorioContrato.ObtenerContratos();
            var lista = repositorioPago.ObtenerPagosPorContratos(id);
            return View("Index",lista);
        }

        // GET: Pago/Edit/5
        [Authorize]
        public ActionResult RealizarPago(int id)
        {
            var lista = repositorioPago.ObtenerPago(id);
            lista.FechaEmision = DateTime.Now;
            ViewBag.contrato = repositorioContrato.ObtenerContratos();
            return View("create",lista);
        }
    }
}