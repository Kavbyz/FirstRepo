using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Store.Models;

namespace Store.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.HeadingsList = db.Headings.ToList();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.HeadingsList = db.Headings.ToList();
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.HeadingsList = db.Headings.ToList();
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult PaymentAndDelivery()
        {
            ViewBag.HeadingsList = db.Headings.ToList();
            ViewBag.Message = "Payment and Delivery information";

            return View();
        }

    }
}