using CineExplorer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        [HttpGet]
        [Route("api/LocationData/ListLocations")]
        public IEnumerable<Location> ListLocations()
        {
            List<Location> Locations = db.Location.ToList();
            return Locations;
        }

        [ResponseType(typeof(Location))]
        [HttpGet]
        [Route("api/LocationData/FindLocation/{id}")]
        public IHttpActionResult FindLocation(int id)
        {
            Location Location = db.Location.Find(id);
            Debug.WriteLine(Location);
            if (Location == null)
            {
                return NotFound();
            }

            return Ok(Location);
        }
    }
}
