using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.Models;

namespace Net.Controllers
{
    public class InquilinoController : Controller
    {
        RepositorioInquilino repositorio;

        public InquilinoController()
        {
            repositorio = new RepositorioInquilino();
        }
         
        // GET: Inquilino
        public ActionResult Index()
        {
            var lista = repositorio.ObtenerInquilinos();
            return View(lista);
        }

        // GET: Inquilino/Details/5
        public ActionResult Detalles(int id)
        {
            var lista = repositorio.ObtenerInquilino(id);
            return View(lista);
        }

        // GET: Inquilino/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inquilino/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino inquilino)
        {
            try
            {
                int res = repositorio.Alta(inquilino);
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

        // GET: Inquilino/Edit/5
        public ActionResult Editar(int id)
        {
            var lista = repositorio.ObtenerInquilino(id);
            return View(lista);
        }

        // POST: Inquilino/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, IFormCollection collection)
        {
            try
            {
                Inquilino i = repositorio.ObtenerInquilino(id);
                i.Nombre = collection["Nombre"];
                i.Dni = collection["Dni"];
                i.LugarTrabajo = collection["LugarTrabajo"];
                i.Direccion = collection["Direccion"];
                i.Email = collection["Email"];
                i.Telefono = collection["Telefono"];
                repositorio.Actualizar(i);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        // GET: Inquilino/Delete/5
        public ActionResult Eliminar(int id)
        {
            var lista = repositorio.ObtenerInquilino(id);
            return View(lista);
        }

        // POST: Inquilino/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id, IFormCollection collection)
        {
            try
            {
                var res = repositorio.Baja(id);
                if(res > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View();      
                }
            }
            catch
            {
                return View();
            }
        }
    }
}