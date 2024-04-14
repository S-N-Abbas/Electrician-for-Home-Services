using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Electrician_for_Home_Services.Models
{
    public class Person
    {
        [Key]
        public string Username { get; set; }

        [Display(Name = "Display Picture")]
        public string ProfilePic { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Mobile")]
        public string MobileNo { get; set; }

        [Required]
        [Display(Name = "Type")]
        public string PersonType { get; set; }

        public int Experience { get; set; }

        [MaxLength(1000)]
        [Display(Name = "Expertise (Comma Separated)")]
        public string Expertise { get; set; }

        public string Address { get; set; }

        
        public bool IsApproved { get; set; }
    }
}