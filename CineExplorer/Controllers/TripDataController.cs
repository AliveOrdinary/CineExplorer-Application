using CineExplorer.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace CineExplorer.Controllers
{
    public class TripDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all trips in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all trips in the database, including their associated user and locations.
        /// </returns>
        [HttpGet]
        [Route("api/TripsData/ListTrips")]
        public IHttpActionResult ListTrips()
        {
            List<Trip> trips = db.Trip.Include(t => t.User).Include(t => t.Location).ToList();
            return Ok(trips);
        }

        /// <summary>
        /// Returns a particular trip in the system.
        /// </summary>
        /// <param name="id">The trip ID</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A trip in the system matching up to the trip ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        [ResponseType(typeof(Trip))]
        [HttpGet]
        [Route("api/TripsData/FindTrip/{id}")]
        public IHttpActionResult FindTrip(int id)
        {
            Trip trip = db.Trip.Include(t => t.User).Include(t => t.Location).FirstOrDefault(t => t.TripId == id);
            if (trip == null)
            {
                return NotFound();
            }
            return Ok(trip);
        }

        /// <summary>
        /// Adds a trip to the system
        /// </summary>
        /// <param name="trip">JSON FORM DATA of a trip</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Trip ID, Trip Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        [ResponseType(typeof(Trip))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult AddTrip(Trip trip)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            trip.UserId = User.Identity.GetUserId();
            db.Trip.Add(trip);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = trip.TripId }, trip);
        }

        /// <summary>
        /// Deletes a trip from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the trip</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        [ResponseType(typeof(Trip))]
        [Route("api/TripsData/DeleteTrip/{id}")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeleteTrip(int id)
        {
            Trip trip = db.Trip.Find(id);
            if (trip == null)
            {
                return NotFound();
            }

            if (trip.UserId != User.Identity.GetUserId())
            {
                return Unauthorized();
            }

            db.Trip.Remove(trip);
            db.SaveChanges();

            return Ok(trip);
        }

        /// <summary>
        /// Updates a particular trip in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Trip ID primary key</param>
        /// <param name="trip">JSON FORM DATA of a trip</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        [ResponseType(typeof(void))]
        [Route("api/TripsData/UpdateTrip/{id}")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdateTrip(int id, Trip trip)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != trip.TripId)
            {
                return BadRequest();
            }

            if (trip.UserId != User.Identity.GetUserId())
            {
                return Unauthorized();
            }

            db.Entry(trip).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds a location to a trip
        /// </summary>
        /// <param name="tripId">The ID of the trip</param>
        /// <param name="locationId">The ID of the location to add</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        [HttpPost]
        [Authorize]
        [Route("api/TripsData/AddLocationToTrip/{tripId}/{locationId}")]
        public IHttpActionResult AddLocationToTrip(int tripId, int locationId)
        {
            Trip trip = db.Trip.Include(t => t.Location).FirstOrDefault(t => t.TripId == tripId);
            if (trip == null)
            {
                return NotFound();
            }

            if (trip.UserId != User.Identity.GetUserId())
            {
                return Unauthorized();
            }

            Location location = db.Location.Find(locationId);
            if (location == null)
            {
                return NotFound();
            }

            if (!trip.Location.Any(l => l.LocationId == locationId))
            {
                trip.Location.Add(location);
                db.SaveChanges();
            }

            return Ok();
        }

        /// <summary>
        /// Removes a location from a trip
        /// </summary>
        /// <param name="tripId">The ID of the trip</param>
        /// <param name="locationId">The ID of the location to remove</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        [HttpPost]
        [Authorize]
        [Route("api/TripsData/RemoveLocationFromTrip/{tripId}/{locationId}")]
        public IHttpActionResult RemoveLocationFromTrip(int tripId, int locationId)
        {
            Trip trip = db.Trip.Include(t => t.Location).FirstOrDefault(t => t.TripId == tripId);
            if (trip == null)
            {
                return NotFound();
            }

            if (trip.UserId != User.Identity.GetUserId())
            {
                return Unauthorized();
            }

            Location location = db.Location.Find(locationId);
            if (location == null)
            {
                return NotFound();
            }

            if (trip.Location.Any(l => l.LocationId == locationId))
            {
                trip.Location.Remove(location);
                db.SaveChanges();
            }

            return Ok();
        }

        /// <summary>
        /// Returns all trips for a particular user
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: All trips in the database for the particular user
        /// </returns>
        [ResponseType(typeof(IEnumerable<Trip>))]
        [HttpGet]
        [Route("api/TripsData/TripsForUser/{userId}")]
        public IHttpActionResult TripsForUser(string userId)
        {
            List<Trip> trips = db.Trip
                 .Include(t => t.User)
                 .Include(t => t.Location)
                 .Where(t => t.UserId == userId)
                 .ToList();

            return Ok(trips);
        }

        private bool TripExists(int id)
        {
            return db.Trip.Count(e => e.TripId == id) > 0;
        }
    }


}
