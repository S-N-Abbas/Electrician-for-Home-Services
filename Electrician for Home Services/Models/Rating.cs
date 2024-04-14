using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Electrician_for_Home_Services.Models
{
    public class Rating
    {
        [Key]
        [ScaffoldColumn(false)]
        public int RatingID { get; set; }

        [Display(Name = "Rated To")]
        public string RatedToUsername { get; set; }

        [Display(Name = "Rated By")]
        public string RatedByUsername { get; set; }

        [Display(Name = "Value")]
        public int RatingValue { get; set; }

        public DateTime Date { get; set; }
    }
}