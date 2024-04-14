using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Electrician_for_Home_Services.Models;
using Microsoft.AspNet.Identity;

namespace Electrician_for_Home_Services.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private E4HSConnection db = new E4HSConnection();

        // GET: Bookings
        public ActionResult Index()
        {
            if(User.IsInRole("Admin"))
                return View(db.Bookings.ToList());

            string username = User.Identity.GetUserName();

            return View(db.Bookings.Where(b => b.BookingByUsername == username || b.BookingToUsername == username).ToList());
        }

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Booking booking = db.Bookings.Find(id);

            if (booking == null)
            {
                return HttpNotFound();
            }

            string username = User.Identity.GetUserName();

            if(booking.BookingToUsername == username || booking.BookingByUsername == username)
            {
                return View(booking);
            }
            return HttpNotFound();
        }

        // GET: Bookings/Create/id
        public ActionResult Create(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Person electrician = db.People.Find(id);

            if (electrician == null)
            {
                return HttpNotFound();
            }

            if (electrician.PersonType != "Electrician")
            {
                return HttpNotFound();
            }

            ViewBag.BookingTo = electrician.Username;

            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookingID,BookingToUsername,Date")] Booking booking)
        {
            booking.BookingByUsername = User.Identity.GetUserName();
            booking.IsAccepted = false;
            booking.CancelledBy = "";
            booking.CancellationCause = "";
            booking.IsCompleted = false;

            if(booking.Date.Date < DateTime.Today)
            {
                ViewBag.BookingTo = booking.BookingToUsername;
                ViewBag.SelectedDate = booking.Date;
                ModelState.AddModelError("Date", "Selected date is not accepted");
            }
            

            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(booking);
        }

        public ActionResult Accept(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }

            string username = User.Identity.GetUserName();

            if (booking.BookingToUsername != username)
                return HttpNotFound();


            booking.IsAccepted = true;

            db.Entry(booking).State = EntityState.Modified;
            db.SaveChanges();

            ViewBag.Message = "Booking marked as accepted";

            return RedirectToAction("Index");
        }

        // GET: Bookings/Complete/id
        [Authorize]
        public ActionResult Complete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }

            string username = User.Identity.GetUserName();

            if (booking.BookingByUsername != username)
                return HttpNotFound();


            booking.IsCompleted = true;

            db.Entry(booking).State = EntityState.Modified;
            db.SaveChanges();

            ViewBag.Message = "Booking Completed";

            RateReviewViewModel RateNReview = new RateReviewViewModel();
            RateNReview.ToUsername = booking.BookingToUsername;
            return View(RateNReview);
        }

        // POST: Bookings/Complete
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Complete([Bind(Include = "ToUsername,RatingValue,ReviewText")] RateReviewViewModel RateNReview)
        {
            if (ModelState.IsValid)
            {
                string username = User.Identity.GetUserName();

                Rating rating = new Rating();
                rating.RatingValue = RateNReview.RatingValue;
                rating.RatedToUsername = RateNReview.ToUsername;
                rating.RatedByUsername = username;
                rating.Date = DateTime.Now;

                Remark remark = new Remark();
                remark.Remarks = RateNReview.ReviewText;
                remark.RemarkToUsername = RateNReview.ToUsername;
                remark.RemarkByUsername = username;
                remark.Date = DateTime.Now;

                db.Ratings.Add(rating);
                db.Remarks.Add(remark);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

            // GET: Bookings/Cancel/5
            public ActionResult Cancel(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Cancel/5
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel([Bind(Include = "BookingID,BookingToUsername,BookingByUsername,Date,CancellationCause")] Booking booking)
        {
            if(ModelState.IsValid)
            {
                booking.CancelledBy = User.Identity.GetUserName();
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Message = "Booking Cancelled";
                return RedirectToAction("Index");
            }

            return View("Cancel", booking);
           
            
        }

        // GET: Bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
