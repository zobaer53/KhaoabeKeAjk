using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using KhaoabeKeAjk.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KhaoabeKeAjk.Controllers
{
    public class HIshab : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "WcBqNw6gWs8MZPRJ9dBuh2nyDMC4kkoA9oQy6ERZ",
            BasePath = "https://bondhukhaoa-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient? client;

        // GET: HIshab
        public ActionResult Index()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Hishabs");
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            var list = new List<DenaPaona>();
            #pragma warning disable CS8602 // Dereference of a possibly null reference.
            foreach (var item in data)
            {
            #pragma warning disable CS8604 // Possible null reference argument.
                list.Add(JsonConvert.DeserializeObject<DenaPaona>(((JProperty)item).Value.ToString()));
            #pragma warning restore CS8604 // Possible null reference argument.
            }
            #pragma warning restore CS8602 // Dereference of a possibly null reference.
            return View(list);
           
        }
        public ActionResult Team() { return View(); }

        [HttpGet]
        public ActionResult Update()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Update(DenaPaona denaPaona)
        {
            try
            {
                AddHishabToFirebse(denaPaona);
                ModelState.AddModelError(string.Empty, "Added Sucessfully");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

            }
           
            return View();
        }

        private void AddHishabToFirebse(DenaPaona denaPaona)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = denaPaona;
            PushResponse response = client.Push("Hishabs/", data);
            data.id = response.Result.name;
            SetResponse setResponse = client.Set("Hishabs/" + data.id, data);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Hishabs/" + id);
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            DenaPaona data = JsonConvert.DeserializeObject<DenaPaona>(response.Body);
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(DenaPaona d)
        {
            client = new FireSharp.FirebaseClient(config);
            SetResponse response = client.Set("Hishabs/" + d.id, d);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Delete("Hishabs/" + id);
            return RedirectToAction("Index");
        }

    }
}
