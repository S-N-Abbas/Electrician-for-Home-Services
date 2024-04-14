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
    public class RemarksController : Controller
    {
        private E4HSConnection db = new E4HSConnection();

        // GET: Remarks/Add
        [Authorize]
        [ChildActionOnly]
        public ActionResult Add(string id)
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

            ViewBag.RemarksTo = electrician.Username;

            return PartialView();
        }

        // POST: Remarks/Add
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ChildActionOnly]
        public ActionResult Add([Bind(Include = "RemarkID,RemarkToUsername,Remarks")] Remark remark)
        {
            remark.RemarkByUsername = User.Identity.GetUserName();
            remark.Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Remarks.Add(remark);
                db.SaveChanges();
            }

            return PartialView("Add");
        }

        // GET: Remarks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Remark remark = db.Remarks.Find(id);
            if (remark == null)
            {
                return HttpNotFound();
            }
            return View(remark);
        }

        // POST: Remarks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Remark remark = db.Remarks.Find(id);
            db.Remarks.Remove(remark);
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
