using CineExplorer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
        public async Task<ActionResult> Details(int id)
        {
            HttpResponseMessage locationResponse = await client.GetAsync($"LocationData/FindLocation/{id}");
            if (locationResponse.IsSuccessStatusCode)
            {
                var location = await locationResponse.Content.ReadAsAsync<Location>();

                // Fetch reviews for this location
                HttpResponseMessage reviewsResponse = await client.GetAsync($"ReviewData/GetReviewsForLocation/{id}");
                if (reviewsResponse.IsSuccessStatusCode)
                {
                    var reviews = await reviewsResponse.Content.ReadAsAsync<List<Review>>();
                    ViewBag.Reviews = reviews;
                }

                return View(location);
            }
            return HttpNotFound();
        }

        // GET: Location/Create
        [Authorize(Users = "admin@testmail.com")]
        public ActionResult Create()
        {
            string url = "MovieData/ListMovies";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<Movie> Movies = response.Content.ReadAsAsync<IEnumerable<Movie>>().Result;
            return View(Movies);
        }

        // POST: Location/Create
        [HttpPost]
        [Authorize(Users = "admin@testmail.com")]
        public ActionResult Create(Location location, HttpPostedFileBase ImageFile, int[] selectedMovies)
        {
            string url = "AddLocation";

            if (ImageFile != null && ImageFile.ContentLength > 0)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                ImageFile.SaveAs(path);
                location.ImageURL = "/Uploads/" + fileName;
            }

            var locationWithMovies = new
            {
                Location = location,
                MovieIds = selectedMovies
            };

            HttpResponseMessage response = client.PostAsJsonAsync(url, locationWithMovies).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            // If we got this far, something failed, redisplay form
            url = "MovieData/ListMovies";
            HttpResponseMessage movieResponse = client.GetAsync(url).Result;
            if (movieResponse.IsSuccessStatusCode)
            {
                var movies = movieResponse.Content.ReadAsAsync<IEnumerable<Movie>>().Result;
                ViewBag.Movies = movies;
            }
            return View(location);
        }

        // GET: Location/Edit/5
        [Authorize(Users = "admin@testmail.com")]
        public ActionResult Edit(int id)
        {
            string url = $"FindLocation/{id}";
            HttpResponseMessage response = client.GetAsync(url).Result;
            Location location = response.Content.ReadAsAsync<Location>().Result;
            return View(location);
        }

        // POST: Location/Edit/5
        [HttpPost]
        [Authorize(Users = "admin@testmail.com")]
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
        [Authorize(Users = "admin@testmail.com")]
        public ActionResult Delete(int id)
        {
            string url = $"FindLocation/{id}";
            HttpResponseMessage response = client.GetAsync(url).Result;
            Location location = response.Content.ReadAsAsync<Location>().Result;
            return View(location);
        }

        // POST: Location/Delete/5
        [HttpPost]
        [Authorize(Users = "admin@testmail.com")]
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