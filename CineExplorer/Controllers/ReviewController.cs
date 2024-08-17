using CineExplorer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CineExplorer.Controllers
{
    public class ReviewController : Controller
    {
        // GET: Review
        private static readonly HttpClient client;

        static ReviewController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44344/api/");
        }

        // GET: Review/Create/{locationId}
        [Authorize]
        public ActionResult Create(int locationId)
        {
            ViewBag.LocationId = locationId;
            return View();
        }

        // POST: Review/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Review review, int locationId)
        {
            if (ModelState.IsValid)
            {
                review.LocationId = locationId;
                var response = await client.PostAsJsonAsync($"ReviewData/AddReviewForLocation/{locationId}", review);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Details", "Location", new { id = locationId });
                }
            }

            ViewBag.LocationId = locationId;
            return View(review);
        }
    }
}