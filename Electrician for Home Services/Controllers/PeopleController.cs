using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Electrician_for_Home_Services.Models;
using System.Web.Security;
using System.Web.WebSockets;

namespace Electrician_for_Home_Services.Controllers
{
    public class PeopleController : Controller
    {
        private E4HSConnection db = new E4HSConnection();

        // GET: People
        // Only returns Person of Type "Electrician"
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var People = db.People.Where(x => x.IsApproved == true);
            return View(People.ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Requests()
        {
            var people = db.People.Where(x => x.IsApproved == false);
            return View("Index", people.ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Approve(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = db.People.Find(id);

            if (user == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            if (!user.IsApproved)
            {
                user.IsApproved = true;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Electricians(string filter, string search)
        {
            var Electricians = db.People.Where(x => x.PersonType == "Electrician" && x.IsApproved == true);

            if (filter != null)
            {
                Electricians = Electricians.Where(x => x.Expertise.Contains(filter));
            }
            
            if(search != null)
            {
                Electricians = Electricians.Where(x => 
                    x.Username.Contains(search)
                    || x.Expertise.Contains(search)
                    || x.FirstName.Contains(search)
                    || x.LastName.Contains(search)
                    || x.Address.Contains(search)
                );
            }

            string uname = User.Identity.GetUserName();

            Electricians = Electricians.Where(x => x.Username != uname);

            return View("Index", Electricians.ToList());
        }

        [Authorize]
        public ActionResult MyProfile()
        {
            string userName = User.Identity.GetUserName();
            var Profile = db.People.Find(userName);
            return PartialView(Profile);
        }

        // GET: People/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Person person = db.People.Find(id);

            if (User.Identity.IsAuthenticated && User.Identity.GetUserName() == person.Username)
            {
                return Redirect("~/Manage/Index");
            }

            else if (person.PersonType == "Electrician")
                return View(person);

            
            else if (User.IsInRole("Admin"))
            {
                return View(person);
            }
            else return View("Error");
        }

        // GET: People/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Username,ProfilePic,FirstName,LastName,MobileNo,PersonType,Experience,Expertise,Address")] Person person, HttpPostedFileBase Thumbnail)
        {
            person.Username = User.Identity.GetUserName();

            person.IsApproved = false;

            if (person.PersonType == "Customer")
                person.Experience = 0;

            if(Thumbnail != null)
            {
                // Uploading Profile Picture
                string pic = System.IO.Path.GetFileName(Thumbnail.FileName);
                // Adding Username to pic name
                pic = User.Identity.GetUserName() + "-" + pic;

                string path = System.IO.Path.Combine(Server.MapPath("~/Profiles/"), pic);

                Thumbnail.SaveAs(path);

                person.ProfilePic = "/Profiles/" + pic;

            }

            if (ModelState.IsValid)
            {
                db.People.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index", "Manage");
            }

            return View(person);
        }

        // GET: People/Edit/5
        [Authorize]
        public ActionResult Edit(string id)
        {
            Person person;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else if (!User.IsInRole("Admin"))
            {
                // Returning his own profile
                person = db.People.Find(User.Identity.GetUserName());
                return View(person);
            }

            // Otherwise if user is admin
            person = db.People.Find(id);
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Username,ProfilePic,FirstName,LastName,MobileNo,PersonType,Experience,Expertise,Address")] Person person, HttpPostedFileBase Thumbnail)
        {
            if (Thumbnail != null)
            {
                // Uploading Profile Picture
                string pic = System.IO.Path.GetFileName(Thumbnail.FileName);
                // Adding Username to pic name
                pic = User.Identity.GetUserName() + "-" + pic;

                string path = System.IO.Path.Combine(Server.MapPath("~/Profiles/"), pic);

                Thumbnail.SaveAs(path);

                person.ProfilePic = "/Profiles/" + pic;
            }

            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(person);
        }

        [ChildActionOnly]
        public ActionResult ViewRating(string id)
        {
            return PartialView(db.Ratings.Where(r => r.RatedToUsername == id));
        }

        [ChildActionOnly]
        public ActionResult ViewJustRating(string id)
        {
            return PartialView(db.Ratings.Where(r => r.RatedToUsername == id));
        }

        [ChildActionOnly]
        public ActionResult ViewRemarks(string id)
        {
            return PartialView(db.Remarks.Where(r => r.RemarkToUsername == id).OrderBy(r => r.Date));
        }

        // GET: People/Delete/5
        [Authorize]
        public ActionResult Delete(string id)
        {
            Person person;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else if (!User.IsInRole("Admin"))
            {
                // Returning his own profile
                person = db.People.Find(User.Identity.GetUserName());
                return View(person);
            }

            // Otherwise if user is admin
            person = db.People.Find(id);
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(string id)
        {
            Person person = db.People.Find(id);
            db.People.Remove(person);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
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
