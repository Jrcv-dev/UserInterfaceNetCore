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
    public class ProductoController : Controller
    {
        string Baseurl = "http://productsapiteam2.azurewebsites.net/";
        // GET: Producto
        public ActionResult Index()
        {
            return View();
        }

        // GET: Producto/Details/5
        public ActionResult Details(int id)
        {
            //Consumir api para obtener un solo producto
            var client = new HttpClient();
            client.BaseAddress = new Uri(Baseurl);
            var response = client.GetAsync("/api/products/" + id.ToString());
            response.Wait();
            var result = response.Result;
            var readresult = result.Content.ReadAsStringAsync().Result;
            var resultadoFinal = JsonConvert.DeserializeObject<Producto>(readresult);
            TempData["username"] = JsonConvert.DeserializeObject(HttpContext.Session.GetString("SessionUser"));
            TempData["rol"] = JsonConvert.DeserializeObject(HttpContext.Session.GetString("UserRol"));
            return View(resultadoFinal);

        }

        // GET: Producto/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Producto/Create
        [HttpPost]
        public ActionResult Create(Producto model)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(Baseurl);
                //Mandamos la peticion por el body hacia la api de security
                string json = JsonConvert.SerializeObject(model);
                var httpcontent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PostAsync("api/products", httpcontent);
                response.Wait();
                var result = response.Result;
                var readresult = result.Content.ReadAsStringAsync().Result;
                //var resultadoFinal = JsonConvert.DeserializeObject<Boolean>(readresult);
                TempData["username"] = JsonConvert.DeserializeObject(HttpContext.Session.GetString("SessionUser"));
                TempData["rol"] = JsonConvert.DeserializeObject(HttpContext.Session.GetString("UserRol"));
                return RedirectToAction("Todos", "Producto", new { id = 1 });
            }
            catch
            {
                return View();
            }
        }

        // GET: Producto/Edit/5
        public ActionResult Edit(int id)
        {

            return View("Edit");
        }

        // POST: Producto/Edit/5
        [HttpPost]
        public ActionResult Edit(Producto model, int id)
        {
            try
            {
                // TODO: Add update logic here
                model.IdProduct = id;
                var client = new HttpClient();
                client.BaseAddress = new Uri(Baseurl);
                //Mandamos la peticion por el body hacia la api de security
                string json = JsonConvert.SerializeObject(model);
                var httpcontent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PutAsync("api/products/" + model.IdProduct, httpcontent);
                response.Wait();
                var result = response.Result;
                var readresult = result.Content.ReadAsStringAsync().Result;
                TempData["username"] = JsonConvert.DeserializeObject(HttpContext.Session.GetString("SessionUser"));
                TempData["rol"] = JsonConvert.DeserializeObject(HttpContext.Session.GetString("UserRol"));
                return RedirectToAction("Todos", "Producto", new { id = 1 });
            }
            catch
            {
                return View();
            }
        }

        // GET: Producto/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: Producto/Delete/5
        //[HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                var client = new HttpClient();
                client.BaseAddress = new Uri(Baseurl);
                //Mandamos la peticion por el body hacia la api de security
                string json = JsonConvert.SerializeObject(id);
                var httpcontent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.DeleteAsync("api/products/" + id);
                response.Wait();
                var result = response.Result;
                var readresult = result.Content.ReadAsStringAsync().Result;
                TempData["username"] = JsonConvert.DeserializeObject(HttpContext.Session.GetString("SessionUser"));
                TempData["rol"] = JsonConvert.DeserializeObject(HttpContext.Session.GetString("UserRol"));
                return RedirectToAction("Todos", "Producto", new { id = 1 });
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Todos(int id)
        {
            //if (Session["username"] == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            List<Producto> products = new List<Producto>();
            //Session["idPage"] = id;
            //Consumir api para obtener los productos
            var client = new HttpClient();
            client.BaseAddress = new Uri(Baseurl);
            var response = client.GetAsync("/api/products/page/" + id.ToString());
            response.Wait();
            var result = response.Result;
            var readresult = result.Content.ReadAsStringAsync().Result;
            var resultadoFinal = JsonConvert.DeserializeObject<List<Producto>>(readresult);//Modelo productos


            //Consumir api para obtener el número de páginas
            var clientpag = new HttpClient();
            clientpag.BaseAddress = new Uri(Baseurl);
            var responsepag = clientpag.GetAsync("/api/products/page/numPages");
            responsepag.Wait();
            var resultpag = responsepag.Result;
            var readresultpag = resultpag.Content.ReadAsStringAsync().Result;
            int numPaginas = Convert.ToInt32(readresultpag);
            //var numPaginas = (int) JsonConvert.DeserializeObject<int>(readresult);//Número de páginas


            var tupleData = new Tuple<List<Producto>, int>(resultadoFinal, numPaginas);
            return View(tupleData);

            //return View(resultadoFinal);
        }
        [HttpGet]
        public ActionResult Logout()
        {
            //Session.Remove("username");
            //Session.Remove("rol");
            //Session.Remove("idPage");
            return RedirectToAction("Index", "Home");
        }

        public ActionResult RedireccionarACambiarPass()
        {
            return RedirectToAction("ChangePassword", "ChangePassword");
        }

        public ActionResult SearchProduct(List<Producto> productos, string name)
        {
            IEnumerable<Producto> productoBuscado = productos.Where(x => x.Name == name).Select(x => new Producto
            {
                Name = x.Name,
                IdProduct = x.IdProduct,
                image = x.image,
                description = x.description,
                price = x.price
            });
            return View("Todos", productoBuscado);
        }
        public IActionResult retView()
        {
            return View();
        }
    }
}