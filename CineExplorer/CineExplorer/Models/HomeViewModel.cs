using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CineExplorer.Models
{
    public class HomeViewModel

    {
        public List<Movie> FeaturedMovies { get; set; }
        public List<Location> FeaturedLocations { get; set; }
        public List<Trip> UserTrips { get; set; }
    }
}