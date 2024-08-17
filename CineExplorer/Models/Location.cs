using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CineExplorer.Models
{
    public class Location
    {
        [Key]
        public int LocationId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ImageURL { get; set; }

        [Display(Name = "Upload Image")]
        public HttpPostedFileBase ImageFile { get; set; }

        //A location can belong to multiple trips
        public ICollection<Trip> Trips { get; set; }

        //A location can belong to multiple Movies
        public ICollection<Movie> Movies { get; set; }

        public Location()
        {
            Movies = new HashSet<Movie>();
        }



    }

}