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
    public class RatingsController : Controller
    {
        private E4HSConnection db = new E4HSConnection();

        // GET: Ratings/Rate
        [Authorize]
        [ChildActionOnly]
        public ActionResult Rate(string id)
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

            ViewBag.RatedTo = electrician.Username;

            return PartialView();
        }

        // POST: Ratings/Rate
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ChildActionOnly]
        public ActionResult Rate([Bind(Include = "RatingID,RatedToUsername,RatingValue")] Rating rating)
        {
            rating.RatedByUsername = User.Identity.GetUserName();
            rating.Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Ratings.Add(rating);
                db.SaveChanges();
            }

            return PartialView("Rate");
        }

        // GET: Ratings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // POST: Ratings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rating rating = db.Ratings.Find(id);
            db.Ratings.Remove(rating);
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
