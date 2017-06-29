using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Store.Models;
using System.Threading.Tasks;
using Store.Filters;

namespace Store.Controllers
{
    [Culture]
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
        [HttpPost]
        public async Task<ActionResult> SearchProducts(string ProdName)
        {
            List<Product> p = db.Products.Where(i => i.Name.Contains(ProdName)).ToList();
            ViewBag.ProdName = ProdName;
            ViewBag.HeadingsList = db.Headings.ToList();
            return View(p);
        }

        public ActionResult ChangeCulture(string lang)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath;
            // Список культур
            List<string> cultures = new List<string>() { "ru", "en", "uk" };
            if (!cultures.Contains(lang))
            {
                lang = "ru";
            }
            // Сохраняем выбранную культуру в куки
            HttpCookie cookie = Request.Cookies["lang"];
            if (cookie != null)
                cookie.Value = lang;   // если куки уже установлено, то обновляем значение
            else
            {

                cookie = new HttpCookie("lang");
                cookie.Value = lang;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return Redirect(returnUrl);
        }

    }
}