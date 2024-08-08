using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CineExplorer.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }
        [Required]
        public string Name { get; set; }
        public int? ReleaseYear { get; set; }
        public string Description { get; set; }
        public bool ImageURL { get; set; }

        //A movie can have multiple locations
        public ICollection<Location> Locations { get; set; }
    }
}