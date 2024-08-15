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
    public class MovieDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        [Route("api/MovieData/ListMovies")]
        public IEnumerable<Movie> ListMovies()
        {
            List<Movie> movies = db.Movie.ToList();
            return movies;
        }

        // GET: api/MovieData/FindMovie/5
        [ResponseType(typeof(Movie))]
        [HttpGet]
        [Route("api/MovieData/FindMovie/{id}")]
        public IHttpActionResult FindMovie(int id)
        {
            Movie movie = db.Movie.Include(m => m.Locations).FirstOrDefault(m => m.MovieId == id);

            return Ok(movie);
        }

        // POST: api/MovieData/AddMovie
        [HttpPost]
        [Route("api/MovieData/AddMovie")]
        public IHttpActionResult AddMovie(Movie movie)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                db.Movie.Add(movie);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine($"Error in AddMovie: {ex.Message}");
                return InternalServerError(ex);
            }
        }

        // POST: api/MovieData/UpdateMovie/5
        [ResponseType(typeof(void))]
        [HttpPost]
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

        // POST: api/MovieData/DeleteMovie/5
        [HttpPost]
        [Route("MovieData/DeleteMovie/{id}")]
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

        private bool MovieExists(int id)
        {
            return db.Movie.Count(e => e.MovieId == id) > 0;
        }
    }
}
