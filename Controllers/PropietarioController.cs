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
    public class PropietarioController : Controller
    {
        RepositorioPropietario repositorio;

        public PropietarioController()
        {
            repositorio = new RepositorioPropietario();
        }

        // GET: Propietario
        [Authorize]
        public ActionResult Index()
        {
            var lista = repositorio.ObtenerPropietarios();
            return View(lista);
        }

        // GET: Propietario/Details/5
        [Authorize]
        public ActionResult Detalles(int id)
        {
            var lista = repositorio.ObtenerPropietario(id);
            return View(lista);
        }

        // GET: Propietario/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Propietario/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario p)
        {
            try
            {
                int res = repositorio.Alta(p);
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

        // GET: Propietario/Edit/5
        [Authorize]
        public ActionResult Editar(int id)
        {
            var lista = repositorio.ObtenerPropietario(id);
            return View(lista);
        }

        // POST: Propietario/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, IFormCollection collection)
        {
            try
            {
                Propietario p = repositorio.ObtenerPropietario(id);
                p.Nombre = collection["Nombre"];
                p.Apellido = collection["Apellido"];
                p.Dni = collection["Dni"];
                p.Telefono = collection["Telefono"];
                p.Email = collection["Email"];
                p.Clave = collection["Clave"];
                repositorio.Actualizar(p);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Propietario/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Eliminar(int id)
        {
            var lista = repositorio.ObtenerPropietario(id);
            return View(lista);
        }

        // POST: Propietario/Delete/5
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id, IFormCollection collection)
        {
            try
            {
                repositorio.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}