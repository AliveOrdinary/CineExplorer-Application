using CineExplorer.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CineExplorer.Controllers
{
    public class TripController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static TripController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44344/api/");
        }

        // GET: Trips
        public ActionResult Index()
        {
            string url = "TripsData/ListTrips";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<Trip> trips = response.Content.ReadAsAsync<IEnumerable<Trip>>().Result;
            return View(trips);
        }

        // GET: Trips/Show/5
        public ActionResult Show(int id)
        {
            string url = "TripData/FindTrip/"+ id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Trip trip = response.Content.ReadAsAsync<Trip>().Result;

            return View(trip);
        }

        // GET: Trips/Create
        [Authorize]
        public ActionResult New()
        {
            string url = "LocationData/ListLocations";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<Location> Locations = response.Content.ReadAsAsync<IEnumerable<Location>>().Result;
            return View(Locations);
        }

        // POST: Trips/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Trip trip, int[] selectedLocations)
        {

            List<Location> location = new List<Location>();
            foreach (var locationId in selectedLocations)
            {
                string url1 = "LocationData/FindLocation/" + locationId;
                HttpResponseMessage response1 = client.GetAsync(url1).Result;
                Location locationData = response1.Content.ReadAsAsync<Location>().Result;
                location.Add(locationData);
            }

            trip.Location = location;
            trip.UserId = User.Identity.GetUserId();
            string url = "TripData/AddTrip";
            
            string jsonpayload = jss.Serialize(trip);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("New");
            }
            else
            {
                return View(trip);
            }
        }

        // GET: Trips/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            string url = $"TripsData/FindTrip/{id}";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Trip trip = response.Content.ReadAsAsync<Trip>().Result;
            return View(trip);
        }

        // POST: Trips/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Trip trip)
        {
            string url = $"TripsData/UpdateTrip/{id}";

            string jsonpayload = jss.Serialize(trip);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(trip);
            }
        }

        // GET: Trips/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            string url = $"TripsData/FindTrip/{id}";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Trip trip = response.Content.ReadAsAsync<Trip>().Result;
            return View(trip);
        }

        // POST: Trips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            string url = $"TripsData/DeleteTrip/{id}";
            HttpResponseMessage response = client.PostAsync(url, null).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
            }
        }

       

    }
}