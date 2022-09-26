using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.Models;
using Microsoft.AspNetCore.Authorization;

namespace Net.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        public RepositorioUsuario repositorioUsuario;

        // GET: Usuario

        public UsuarioController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
            repositorioUsuario = new RepositorioUsuario();
        }

        // GET: Usuario
        [Authorize(Policy = "Administrador")]
        public ActionResult Index()
        {
            var lista = repositorioUsuario.ObtenerUsuarios();
            return View(lista);
        }

        // GET: Usuario/Details/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Detalles(int id)
        {
            var lista = repositorioUsuario.ObtenerUsuario(id);
            return View(lista);
        }

        // GET: Usuario/Create
        [Authorize(Policy = "Administrador")]
        public ActionResult Create()
        {
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View();
        }

        // POST: Usuario/Create
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario usuario)
        {
            if (ModelState.IsValid)
                return View();
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: usuario.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                usuario.Clave = hashed;
                repositorioUsuario.Alta(usuario);
                if(usuario.AvatarFile != null && usuario.IdUsuario > 0)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath,"Upload");
                    string fileName = "avatar_" + usuario.IdUsuario + Path.GetExtension(usuario.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path,fileName);
                    usuario.Avatar = Path.Combine("/Upload",fileName);
                    using(FileStream stream = new FileStream(pathCompleto,FileMode.Create))
                    {
                         usuario.AvatarFile.CopyTo(stream);
                    }
                    repositorioUsuario.Actualizar(usuario);
                }

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        // GET: Usuario/Edit/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Editar(int id)
        {
            ViewBag.Roles = Usuario.ObtenerRoles();
            var lista = repositorioUsuario.ObtenerUsuario(id);
            return View(lista);
        }

        // POST: Usuario/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, Usuario usuario)
        {
            var vista = nameof(Editar);
            Usuario user;

            try
            {
                
                if(!User.IsInRole("Administrador"))
                {
                    vista = nameof(Perfil);
                    user = repositorioUsuario.ObtenerUsuarioPorEmail(User.Identity.Name);

                    if(user.IdUsuario != id)
                    {
                        user.Nombre = usuario.Nombre;
                        user.Apellido = usuario.Apellido;
                        user.Email = usuario.Email;

                        if(!String.IsNullOrEmpty(usuario.Clave))
                        {
                            if(!usuario.Clave.Equals(user.Clave))
                            {
                                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: usuario.Clave,
                                salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                                prf: KeyDerivationPrf.HMACSHA1,
                                iterationCount: 1000,
                                numBytesRequested: 256 / 8));
                                user.Clave = hashed;
                            }
                        }

                        if(user.AvatarFile != null)
                        {
                            string wwwPath = environment.WebRootPath;
                            string path = Path.Combine(wwwPath,"Upload");
                            string fileName = "avatar_" + user.IdUsuario + Path.GetExtension(usuario.AvatarFile.FileName);
                            string pathCompleto = Path.Combine(path,fileName);
                            user.Avatar = Path.Combine("/Upload",fileName);
                            using(FileStream stream = new FileStream(pathCompleto,FileMode.Create))
                            {
                                usuario.AvatarFile.CopyTo(stream);
                            }
                        }
                    }        
                    repositorioUsuario.Actualizar(user);
                    return RedirectToAction(nameof(Index),"Home");        
                }
                else
                {
                    

                    user = repositorioUsuario.ObtenerUsuario(id);
                    user.Nombre = usuario.Nombre;
                    user.Apellido = usuario.Apellido;
                    user.Email = usuario.Email;

                    if(!String.IsNullOrEmpty(usuario.Clave))
                    {
                        if(!usuario.Clave.Equals(user.Clave))
                        {
                            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: usuario.Clave,
                            salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                            prf: KeyDerivationPrf.HMACSHA1,
                            iterationCount: 1000,
                            numBytesRequested: 256 / 8));
                            user.Clave = hashed;
                        }
                    }

                    if(user.AvatarFile != null)
                    {
                        string wwwPath = environment.WebRootPath;
                        string path = Path.Combine(wwwPath,"Upload");
                        string fileName = "avatar_" + user.IdUsuario + Path.GetExtension(usuario.AvatarFile.FileName);
                        string pathCompleto = Path.Combine(path,fileName);
                        user.Avatar = Path.Combine("/Upload",fileName);
                        using(FileStream stream = new FileStream(pathCompleto,FileMode.Create))
                        {
                            usuario.AvatarFile.CopyTo(stream);
                        }
                    }
                    repositorioUsuario.Actualizar(user);
                    return RedirectToAction(vista);
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        // GET: Usuario/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Eliminar(int id)
        {
            var lista = repositorioUsuario.ObtenerUsuario(id);
            return View(lista);
        }

        // POST: Usuario/Delete/5
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id, IFormCollection collection)
        {
            try
            {
                repositorioUsuario.Baja(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



        // GET: Usuario/Login/Url

        [AllowAnonymous]
        public ActionResult Login(String returnUrl)
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        } 


        // POST : Usuario/Login/

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginView login)
        {
            try
            {
                var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as String)? "/Home" : TempData["returnUrl"].ToString();
                if(ModelState.IsValid)
                {
                    String hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                       password: login.Clave,
                       salt : System.Text.Encoding.ASCII.GetBytes(configuration["salt"]),
                       prf : KeyDerivationPrf.HMACSHA1,
                       iterationCount : 1000,
                       numBytesRequested : 256 / 8 
                    ));

                    Usuario user = repositorioUsuario.ObtenerUsuarioPorEmail(login.Usuario);
                    if(user == null || user.Clave != hashed)
                    {
                        ModelState.AddModelError("","El email o la clave ingresada es invalida");
                        TempData["returnUrl"] = returnUrl;
                        return View();
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim("FullName", user.Nombre + " " + user.Apellido),
                        new Claim(ClaimTypes.Role, user.RolNombre),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                    TempData.Remove(returnUrl);
                    return Redirect(returnUrl);
                }
                TempData["returnUrl"] = returnUrl;
                return Redirect(returnUrl);
            }
            catch(Exception ex)
            {
                throw;
            }
        }


        [Route("salir", Name = "logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // GET: Usuario/Details/5
        [Authorize]
        public ActionResult Perfil()
        {
            var lista = repositorioUsuario.ObtenerUsuarioPorEmail(User.Identity.Name);
            return View("Editar",lista);
        }

    }
}