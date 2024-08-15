using CineExplorer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CineExplorer.Controllers
{
    public class LocationController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static LocationController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44344/api/LocationData/");
        }

        // GET: Location
        public ActionResult Index()
        {
            string url = "ListLocations";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<Location> locations = response.Content.ReadAsAsync<IEnumerable<Location>>().Result;
            return View(locations);
        }

        // GET: Location/Details/5
        public ActionResult Details(int id)
        {
            string url = $"FindLocation/{id}";
            HttpResponseMessage response = client.GetAsync(url).Result;
            Location location = response.Content.ReadAsAsync<Location>().Result;
            return View(location);
        }

        // GET: Location/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Location/Create
        [HttpPost]
        public ActionResult Create(Location location)
        {
            string url = "AddLocation";
            string jsonpayload = jss.Serialize(location);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(location);
            }
        }

        // GET: Location/Edit/5
        public ActionResult Edit(int id)
        {
            string url = $"FindLocation/{id}";
            HttpResponseMessage response = client.GetAsync(url).Result;
            Location location = response.Content.ReadAsAsync<Location>().Result;
            return View(location);
        }

        // POST: Location/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Location location)
        {
            string url = $"UpdateLocation/{id}";
            string jsonpayload = jss.Serialize(location);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PutAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(location);
            }
        }

        // GET: Location/Delete/5
        public ActionResult Delete(int id)
        {
            string url = $"FindLocation/{id}";
            HttpResponseMessage response = client.GetAsync(url).Result;
            Location location = response.Content.ReadAsAsync<Location>().Result;
            return View(location);
        }

        // POST: Location/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Location location)
        {
            string url = $"DeleteLocation/{id}";
            HttpResponseMessage response = client.DeleteAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(location);
            }
        }
    }
}