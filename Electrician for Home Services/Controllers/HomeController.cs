using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Electrician_for_Home_Services.Models;

namespace Electrician_for_Home_Services.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Electricians for Home Services";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Feel Free to Contact Us";

            return View();
        }
    }
}