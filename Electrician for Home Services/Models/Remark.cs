using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Electrician_for_Home_Services.Models
{
    public class Remark
    {
        [Key]
        [ScaffoldColumn(false)]
        public int RemarkID { get; set; }

        [Display(Name = "Remark To")]
        public string RemarkToUsername { get; set; }

        [Display(Name = "Remark By")]
        public string RemarkByUsername { get; set; }

        [MaxLength(10000)]
        public string Remarks { get; set; }

        public DateTime Date { get; set; }
    }
}