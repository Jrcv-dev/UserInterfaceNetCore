using System;
using System.Collections.Generic;
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
    public class ChangePasswordController : Controller
    {
        public ActionResult ChangePassword()
        {
            TempData["username"] = JsonConvert.DeserializeObject(HttpContext.Session.GetString("SessionUser"));
            //Vista de cambiar Password
            var usuario = new ChangePassword { UserName = TempData["username"].ToString() };
            TempData["rol"] = JsonConvert.DeserializeObject(HttpContext.Session.GetString("UserRol"));
            return View(usuario);
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePassword model)
        {
            //proceso de la api
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://academysecuritydos.azurewebsites.net/");
            //Mandamos la peticion por el body hacia la api de security
            string json = JsonConvert.SerializeObject(model);
            var httpcontent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync("api/NewPassword/ChangePassword", httpcontent);
            response.Wait();
            var result = response.Result;
            var readresult = result.Content.ReadAsStringAsync().Result;
            var resultadoFinal = JsonConvert.DeserializeObject<Boolean>(readresult);
            if (resultadoFinal == true)
            {
                return RedirectToAction("Todos", "Producto", new { id = 1 });
            }
            else
            {
                //en dado caso que falle el cambiar contraseña que hacemos?
                return View();
            }
        }
    }
}