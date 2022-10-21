using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Inmobiliaria_2022.Models;

namespace Inmobiliaria_2022.Controllers
{
    public class InmuebleController : Controller
    {
        RepositorioInmueble repositorioInmueble;
        RepositorioPropietario repositorioPropietario;

        public InmuebleController()
        {
            repositorioPropietario = new RepositorioPropietario();
            repositorioInmueble = new RepositorioInmueble();
        }


        // GET: Inmueble
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.VistaPropietario = false;
            var lista = repositorioInmueble.ObtenerInmuebles();
            return View(lista);
        }

        // GET: Inmueble/Details/5
        [Authorize]
        public ActionResult Detalles(int id)
        {                         
            var lista = repositorioInmueble.ObtenerInmueble(id);                  
            return View(lista);
        }

        // GET: Inmueble/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Propietario = repositorioPropietario.ObtenerPropietarios();
            return View();
        }

        // POST: Inmueble/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble i)
        {
            try
            {
                int res = repositorioInmueble.Alta(i);
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

        // GET: Inmueble/Edit/5
        [Authorize]
        public ActionResult Editar(int id)
        {
            ViewBag.Propietario = repositorioPropietario.ObtenerPropietarios();
            ViewBag.inmueble = repositorioInmueble.ObtenerInmueble(id);
            var lista = repositorioInmueble.ObtenerInmueble(id);             
            return View(lista);
        }

        // POST: Inmueble/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, IFormCollection collection)
        {
            try
            {
                Inmueble i = repositorioInmueble.ObtenerInmueble(id);
                i.IdPropietario = Int32.Parse(collection["IdPropietario"]);
                i.Direccion = collection["Direccion"];
                i.Tipo = collection["Tipo"];
                i.Uso = collection["Uso"];
                i.Ambientes = Int32.Parse(collection["Ambientes"]);
                i.Longitud = collection["Longitud"];
                i.Latitud = collection["Latitud"];
                i.Precio = Double.Parse(collection["Precio"]);
                if(collection["Estado"].Equals("true"))
                {
                    if(i.Estado == false)
                    {
                        i.Estado = true;
                    }
                }
                else
                {
                    if(i.Estado == true)
                    {
                        i.Estado = false;
                    }
                }
                repositorioInmueble.Actualizar(i);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        // GET: Inmueble/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Eliminar(int id)
        {
            var lista = repositorioInmueble.ObtenerInmueble(id);                  
            return View(lista);
        }

        // POST: Inmueble/Delete/5
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id, IFormCollection collection)
        {
            try
            {
                repositorioInmueble.Baja(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        // GET: Inmueble
        [Authorize]
        public ActionResult ListarDisponible(int valor)
        {
            ViewBag.VistaPropietario = false;
            var lista = repositorioInmueble.ObtenerInmueblesDisponibles(valor);
            return View("Index",lista);
        }

        // GET: Inmueble
        [Authorize]
        public ActionResult ListarDisponiblePorPropietario(int id)
        {
            ViewBag.VistaPropietario = true;
            var lista = repositorioInmueble.ObtenerInmueblesPorPropietario(id);
            return View("Index",lista);
        }
    }
}