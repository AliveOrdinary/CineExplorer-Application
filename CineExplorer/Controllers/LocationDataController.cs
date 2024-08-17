using CineExplorer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace CineExplorer.Controllers
{
    public class LocationDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieves a list of all locations.
        /// </summary>
        /// <returns>A list of locations.</returns>
        [HttpGet]
        [Route("api/LocationData/ListLocations")]
        public IEnumerable<Location> ListLocations()
        {
            List<Location> locations = db.Location.ToList();
            return locations;
        }

        /// <summary>
        /// Retrieves a specific location by its ID, including its associated movies.
        /// </summary>
        /// <param name="id">The ID of the location to retrieve.</param>
        /// <returns>The location with the specified ID</returns>
        // GET: api/LocationData/FindLocation/5
        [ResponseType(typeof(Location))]
        [HttpGet]
        [Route("api/LocationData/FindLocation/{id}")]
        public IHttpActionResult FindLocation(int id)
        {
            Location location = db.Location.Include(l => l.Movies).FirstOrDefault(l => l.LocationId == id);
            if (location == null)
            {
                return NotFound();
            }
            return Ok(location);
        }

        /// <summary>
        /// Adds a new location to the database, along with its associated movies.
        /// Requires admin authorization.
        /// </summary>
        /// <param name="locationWithMovies">An object containing the location details and associated movie IDs.</param>
        /// <returns>The created location with its associated movies, or a 400 Bad Request response if the model state is invalid.</returns>
        // POST: api/LocationData/AddLocation
        [Authorize(Users = "admin@testmail.com")]
        [HttpPost]
        [Route("api/LocationData/AddLocation")]
        public IHttpActionResult AddLocation(LocationWithMovies locationWithMovies)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var location = locationWithMovies.Location;
            db.Location.Add(location);

            if (locationWithMovies.MovieIds != null && locationWithMovies.MovieIds.Any())
            {
                foreach (var movieId in locationWithMovies.MovieIds)
                {
                    var movie = db.Movie.Find(movieId);
                    if (movie != null)
                    {
                        location.Movies.Add(movie);
                    }
                }
            }

            db.SaveChanges();
            return Ok(location);
        }

        /// <summary>
        /// Updates an existing location in the database.
        /// Requires admin authorization.
        /// </summary>
        /// <param name="id">The ID of the location to update.</param>
        /// <param name="location">The updated location details.</param>
        /// <returns>An HTTP status code indicating the result of the update operation.</returns>
        // POST: api/LocationData/UpdateLocation/5
        [Authorize(Users = "admin@testmail.com")]
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/LocationData/UpdateLocation/{id}")]
        public IHttpActionResult UpdateLocation(int id, Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != location.LocationId)
            {
                return BadRequest();
            }
            db.Entry(location).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                if (!LocationExists(id))
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
        /// Deletes a location from the database by its ID.
        /// Requires admin authorization.
        /// </summary>
        /// <param name="id">The ID of the location to delete.</param>
        /// <returns>The deleted location, or a 404 Not Found response if the location does not exist.</returns>
        // POST: api/LocationData/DeleteLocation/5
        [Authorize(Users = "admin@testmail.com")]
        [HttpPost]
        [Route("api/LocationData/DeleteLocation/{id}")]
        public IHttpActionResult DeleteLocation(int id)
        {
            Location location = db.Location.Find(id);
            if (location == null)
            {
                return NotFound();
            }
            db.Location.Remove(location);
            db.SaveChanges();
            return Ok(location);
        }

        private bool LocationExists(int id)
        {
            return db.Location.Count(e => e.LocationId == id) > 0;
        }

        public class LocationWithMovies
        {
            public Location Location { get; set; }
            public int[] MovieIds { get; set; }
        }
    }
}