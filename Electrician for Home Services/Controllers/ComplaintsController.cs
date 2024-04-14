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
    public class ComplaintsController : Controller
    {
        private E4HSConnection db = new E4HSConnection();

        // GET: Complaints
        [Authorize]
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
                return View(db.Complaints.ToList());

            string username = User.Identity.GetUserName();
            return View(db.Complaints.Where(c => c.ComplaintByUsername == username).ToList());
        }


        // GET: Complaints/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Complaint complaint = db.Complaints.Find(id);
            if (complaint == null)
            {
                return HttpNotFound();
            }

            if(User.IsInRole("Admin"))
                return View(complaint);

            string username = User.Identity.GetUserName();
            if(complaint.ComplaintByUsername != username)
            {
                return HttpNotFound();
            }

            return View(complaint);
        }

        // GET: Complaints/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Complaints/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "ComplaintID,ComplaintAbout,ComplaintText")] Complaint complaint)
        {
            complaint.ComplaintByUsername = User.Identity.GetUserName();
            complaint.Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Complaints.Add(complaint);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(complaint);
        }

        // GET: Complaints/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Complaint complaint = db.Complaints.Find(id);
            if (complaint == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole("Admin"))
                return View(complaint);

            string username = User.Identity.GetUserName();
            if (complaint.ComplaintByUsername != username)
            {
                return HttpNotFound();
            }

            return View(complaint);
        }

        // POST: Complaints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Complaint complaint = db.Complaints.Find(id);
            db.Complaints.Remove(complaint);
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
