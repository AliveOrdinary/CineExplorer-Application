using CineExplorer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace CineExplorer.Controllers
{
    public class MovieDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieves a list of all movies in the database.
        /// </summary>
        /// <returns>An IEnumerable of Movie objects representing all movies in the database.</returns>
        [HttpGet]
        [Route("api/MovieData/ListMovies")]
        public IEnumerable<Movie> ListMovies()
        {
            List<Movie> movies = db.Movie.ToList();
            return movies;
        }

        /// <summary>
        /// Finds a specific movie by its ID.
        /// </summary>
        /// <param name="id">The ID of the movie to find.</param>
        /// <returns>An IHttpActionResult containing the movie if found, or a NotFound result if not found.</returns>
        [ResponseType(typeof(Movie))]
        [HttpGet]
        [Route("api/MovieData/FindMovie/{id}")]
        public IHttpActionResult FindMovie(int id)
        {
            Movie movie = db.Movie.Include(m => m.Locations).FirstOrDefault(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }

        /// <summary>
        /// Adds a new movie to the database.
        /// </summary>
        /// <param name="movie">The Movie object to add to the database.</param>
        /// <returns>An IHttpActionResult indicating the result of the operation.</returns>
        [HttpPost]
        [Authorize(Users = "admin@testmail.com")]
        [Route("api/MovieData/AddMovie")]
        public IHttpActionResult AddMovie(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Movie.Add(movie);
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = movie.MovieId }, movie);
        }

        /// <summary>
        /// Updates an existing movie in the database.
        /// </summary>
        /// <param name="id">The ID of the movie to update.</param>
        /// <param name="movie">The updated Movie object.</param>
        /// <returns>An IHttpActionResult indicating the result of the operation.</returns>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize(Users = "admin@testmail.com")]
        [Route("api/MovieData/UpdateMovie/{id}")]
        public IHttpActionResult UpdateMovie(int id, Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != movie.MovieId)
            {
                return BadRequest();
            }
            db.Entry(movie).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                if (!MovieExists(id))
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
        /// Deletes a movie from the database.
        /// </summary>
        /// <param name="id">The ID of the movie to delete.</param>
        /// <returns>An IHttpActionResult indicating the result of the operation.</returns>
        [HttpPost]
        [Authorize(Users = "admin@testmail.com")]
        [Route("api/MovieData/DeleteMovie/{id}")]
        public IHttpActionResult DeleteMovie(int id)
        {
            Movie movie = db.Movie.Find(id);
            if (movie == null)
            {
                return NotFound();
            }
            db.Movie.Remove(movie);
            db.SaveChanges();
            return Ok(movie);
        }

        /// <summary>
        /// Checks if a movie with the given ID exists in the database.
        /// </summary>
        /// <param name="id">The ID of the movie to check.</param>
        /// <returns>True if the movie exists, false otherwise.</returns>
        private bool MovieExists(int id)
        {
            return db.Movie.Count(e => e.MovieId == id) > 0;
        }
    }
}