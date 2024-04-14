using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Electrician_for_Home_Services.Models
{
    public class Complaint
    {
        [Key]
        [ScaffoldColumn(false)]
        public int ComplaintID { get; set; }

        [Display(Name = "Complaint About")]
        public string ComplaintAbout { get; set; }

        [Display(Name = "Complaint By")]
        public string ComplaintByUsername { get; set; }

        [MaxLength(10000)]
        public string ComplaintText { get; set; }

        public DateTime Date { get; set; }
    }
}