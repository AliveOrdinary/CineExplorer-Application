using CineExplorer.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CineExplorer.Controllers
{
    public class ReviewDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // ... existing methods ...

        // POST: api/ReviewData/AddReviewForLocation/{locationId}
        [HttpPost]
        [Route("AddReviewForLocation/{locationId}")]
        [Authorize]
        public IHttpActionResult AddReviewForLocation(int locationId, Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var location = db.Location.Find(locationId);
            if (location == null)
            {
                return NotFound();
            }

            review.LocationId = locationId;
            review.UserId = User.Identity.GetUserId();
            review.DateReviewed = DateTime.UtcNow;

            db.Review.Add(review);
            db.SaveChanges();

            return Ok(review);
        }

        // GET: api/ReviewData/GetReviewsForLocation/{locationId}
        [HttpGet]
        [Route("GetReviewsForLocation/{locationId}")]
        public IHttpActionResult GetReviewsForLocation(int locationId)
        {
            var reviews = db.Review
                .Where(r => r.LocationId == locationId)
                .ToList();

            return Ok(reviews);
        }
    }
}
