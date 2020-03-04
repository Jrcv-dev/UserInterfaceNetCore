using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreUserInterface.Models;
using Newtonsoft.Json;

namespace NetCoreUserInterface.Controllers
{
    public class RegistrarUsuarioController : Controller
    {
        public ActionResult RegisterUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegisterUser(RegisterUser model)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://academysecuritydos.azurewebsites.net/");
            //Mandamos la peticion por el body hacia la api de security
            string json = JsonConvert.SerializeObject(model);
            var httpcontent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync("api/NewUser/CreateUser", httpcontent);
            response.Wait();
            var result = response.Result;
            var readresult = result.Content.ReadAsStringAsync().Result;
            //var resultadoFinal = JsonConvert.DeserializeObject<Boolean>(readresult);
            return RedirectToAction("Index", "Home");
        }
    }
}