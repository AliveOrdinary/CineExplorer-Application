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
    public class MovieController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static MovieController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44344/api/MovieData/");
        }

        // GET: Movie
        public ActionResult Index()
        {
            string url = "ListMovies";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<Movie> movies = response.Content.ReadAsAsync<IEnumerable<Movie>>().Result;
            return View(movies);
        }

        // GET: Movie/Details/5
        public ActionResult Details(int id)
        {
            string url = "FindMovie/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Movie movie = response.Content.ReadAsAsync<Movie>().Result;
            return View(movie);
        }

        // GET: Movie/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movie/Create
        [HttpPost]
        public ActionResult Create(Movie movie)
        {
            try
            {
                string url = "AddMovie";
                string jsonpayload = jss.Serialize(movie);
                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";
                HttpResponseMessage response = client.PostAsync(url, content).Result;
                response.EnsureSuccessStatusCode();
                return RedirectToAction("Index");
            }
            catch (HttpRequestException e)
            {
                // Log the error
                System.Diagnostics.Debug.WriteLine($"Error in Create: {e.Message}");
                ModelState.AddModelError("", "An error occurred while creating the movie. Please try again.");
                return View(movie);
            }
        }

        // GET: Movie/Edit/5
        public ActionResult Edit(int id)
        {
            string url = $"FindMovie/{id}";
            HttpResponseMessage response = client.GetAsync(url).Result;
            Movie movie = response.Content.ReadAsAsync<Movie>().Result;
            return View(movie);
        }

        // POST: Movie/Edit/5
        [HttpPut]
        public ActionResult Update(int id, Movie movie)
        {
            string url = "UpdateMovie/"+ id;
            string jsonpayload = jss.Serialize(movie);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PutAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(movie);
            }
        }

        // GET: Movie/Delete/5
        public ActionResult Delete(int id)
        {
            string url = $"FindMovie/{id}";
            HttpResponseMessage response = client.GetAsync(url).Result;
            Movie movie = response.Content.ReadAsAsync<Movie>().Result;
            return View(movie);
        }

        // POST: Movie/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Movie movie)
        {
            string url = $"DeleteMovie/{id}";
            HttpResponseMessage response = client.DeleteAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(movie);
            }
        }
    }
}