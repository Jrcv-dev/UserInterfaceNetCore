using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreUserInterface.Models;
using Newtonsoft.Json;

namespace NetCoreUserInterface.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            List<Producto> lsp = new List<Producto>();
            UsuarioConectado userLoged = new UsuarioConectado();
            Usuario usuario = new Usuario();
            //Las propiedades tienen que hacer match con el DTO para poder comunicarse.
            usuario.UserName = username;
            usuario.Password = password;
            //Consumir api para obtener account y password y validar
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://academysecuritydos.azurewebsites.net/");
            //Mandamos la peticion por el body hacia la api de security
            string json = JsonConvert.SerializeObject(usuario);
            var httpcontent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync("api/Sessions/Login", httpcontent);
            response.Wait();
            var result = response.Result;
            var readresult = result.Content.ReadAsStringAsync().Result;
            var resultadoFinal = JsonConvert.DeserializeObject<UsuarioConectado>(readresult);
            if (resultadoFinal.IsLogged != false)
            {
                HttpContext.Session.SetString("SessionUser", JsonConvert.SerializeObject(resultadoFinal.Username));
                HttpContext.Session.SetString("UserRol", JsonConvert.SerializeObject(resultadoFinal.Role));
                if (HttpContext.Session.GetString("SessionUser") != null)
                {
                    TempData["username"]= JsonConvert.DeserializeObject(HttpContext.Session.GetString("SessionUser"));
                    TempData["rol"] = JsonConvert.DeserializeObject(HttpContext.Session.GetString("UserRol"));
          
                }
                //Crearmos el objeto que recibira nuestro metodo para la llamada a la api
                //LogEntity logUI = new LogEntity();
                //logUI.aplicacion = "User Interface";
                //logUI.mensaje = "El usuario\t" + resultadoFinal.Username + "\tha iniciado sesion";
                //logUI.fecha = DateTime.Now;
                ////Instanciamos el Log para poder consumir el metodo de conexion a la Api del archivo Dll
                //Log log = new Log();
                //log.ConnectToWebAPI(logUI);
                //return PartialView("~/Views/Producto/Todos.cshtml",lsp);
                return RedirectToAction("Todos", "Producto", new { id = 1 });
            }
            else
            {
                ViewBag.error = "Invalid Account";
                return View("Index");
            }
        }

        public ActionResult RegisterUser()
        {
            return RedirectToAction("RegisterUser", "RegistrarUsuario");
        }
    }
}
