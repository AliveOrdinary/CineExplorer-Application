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

        /// <summary>
        /// Adds a new review for a specific location.
        /// </summary>
        /// <param name="locationId">The ID of the location being reviewed.</param>
        /// <param name="review">The Review object containing the review details.</param>
        /// <returns>An IHttpActionResult indicating the result of the operation.</returns>
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

        /// <summary>
        /// Retrieves all reviews for a specific location.
        /// </summary>
        /// <param name="locationId">The ID of the location to get reviews for.</param>
        /// <returns>An IHttpActionResult containing a list of reviews for the specified location.</returns>
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