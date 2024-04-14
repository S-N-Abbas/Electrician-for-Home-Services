using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Electrician_for_Home_Services.Models
{
    public class RateReviewViewModel
    {
        public string ToUsername { get; set; }


        [Display(Name = "Rating")]
        public int RatingValue { get; set; }

        [Display(Name = "Review")]
        public string ReviewText { get; set; }
    }
}