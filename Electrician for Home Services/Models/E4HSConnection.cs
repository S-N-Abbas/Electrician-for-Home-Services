using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Electrician_for_Home_Services.Models
{
    public class E4HSConnection : DbContext
    {
        public E4HSConnection() : base("E4HS")
        {

        }

        public DbSet<Person> People { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Remark> Remarks { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}