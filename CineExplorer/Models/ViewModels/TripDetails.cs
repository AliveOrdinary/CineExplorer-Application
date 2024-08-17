using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CineExplorer.Models.ViewModels
{
    public class TripDetails
    {
       
        public Trip Trip { get; set; }
        public IEnumerable<Location> IncludedLocations { get; set; }
        public IEnumerable<Location> AllLocations { get; set; }
   }
}