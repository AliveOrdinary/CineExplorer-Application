using CineExplorer.Models;
using CineExplorer.Models.ViewModels;
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
    public class HomeController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static HomeController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44344/api/");
        }

        public ActionResult Index()
        {
            var viewModel = new HomeViewModel();

            string movieUrl = "MovieData/ListMovies";
            HttpResponseMessage movieResponse = client.GetAsync(movieUrl).Result;
            if (movieResponse.IsSuccessStatusCode)
            {
                viewModel.FeaturedMovies = movieResponse.Content.ReadAsAsync<IEnumerable<Movie>>().Result.Take(5).ToList();
            }
            else
            {
                viewModel.FeaturedMovies = new List<Movie>(); // Initialize with an empty list if the API call fails
            }

            // Get Locations
            string locationUrl = "LocationData/ListLocations";
            HttpResponseMessage locationResponse = client.GetAsync(locationUrl).Result;
            if (locationResponse.IsSuccessStatusCode)
            {
                viewModel.FeaturedLocations = locationResponse.Content.ReadAsAsync<IEnumerable<Location>>().Result.Take(5).ToList();
            }
            else
            {
                viewModel.FeaturedLocations = new List<Location>(); // Initialize with an empty list if the API call fails
            }

            // Get Trips for the current user
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.Identity.GetUserId();
                string tripUrl = "TripData/ListTripsForUser";
                HttpResponseMessage tripResponse = client.GetAsync(tripUrl).Result;
                if (tripResponse.IsSuccessStatusCode)
                {
                    viewModel.UserTrips = tripResponse.Content.ReadAsAsync<IEnumerable<Trip>>().Result.Take(5).ToList();
                }
                else
                {
                    viewModel.UserTrips = new List<Trip>(); // Initialize with an empty list if the API call fails
                }
            }

            return View(viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}