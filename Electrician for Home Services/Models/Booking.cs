using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Electrician_for_Home_Services.Models
{
    public class Booking
    {
        [Key]
        [ScaffoldColumn(false)]
        public int BookingID { get; set; }

        [Display(Name = "Booking To")]
        public string BookingToUsername { get; set; }

        [Display(Name = "Booking By")]
        public string BookingByUsername { get; set; }

        public DateTime Date { get; set; }

        [ScaffoldColumn(false)]
        public bool IsAccepted { get; set; }

        [Display(Name = "Cancelled By")]
        public string CancelledBy { get; set; }

        [Display(Name = "Cancellation Cause")]
        [MaxLength(10000)]
        public string CancellationCause { get; set; }

        [ScaffoldColumn(false)]
        public bool IsCompleted { get; set; }
    }
}