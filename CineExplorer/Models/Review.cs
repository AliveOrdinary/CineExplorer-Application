using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls.WebParts;

namespace CineExplorer.Models
{
    public class Review
    {
        [Key]
        public int ReviewID { get; set; }

        [Required]
        [StringLength(100)]
        public string ReviewText { get; set; } = string.Empty;

        [Required]
        public DateTime DateReviewed { get; set; } = DateTime.UtcNow;

        // A Location can have many Comments
        [Required]
        [ForeignKey("Location")]
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserID { get; set; }
        public virtual User User { get; set; }
    }
}