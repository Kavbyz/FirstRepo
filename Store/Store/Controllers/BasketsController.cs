using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Store.Models;
using System.Threading;
using Microsoft.AspNet.Identity;

namespace Store.Controllers
{
    public class BasketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Baskets
        public async Task<ActionResult> Index()
        {
            return View(await db.Basket.ToListAsync());
        }

        // GET: Baskets/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Basket basket = await db.Basket.FindAsync(id);
            if (basket == null)
            {
                return HttpNotFound();
            }
            return View(basket);
        }

        // GET: Baskets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Baskets/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id")] Basket basket)
        {
            if (ModelState.IsValid)
            {
                db.Basket.Add(basket);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(basket);
        }

        // GET: Baskets/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Basket basket = await db.Basket.FindAsync(id);
            if (basket == null)
            {
                return HttpNotFound();
            }
            return View(basket);
        }

        // POST: Baskets/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id")] Basket basket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(basket).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(basket);
        }

        // GET: Baskets/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Basket basket = await db.Basket.FindAsync(id);
            if (basket == null)
            {
                return HttpNotFound();
            }
            return View(basket);
        }

        // POST: Baskets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Basket basket = await db.Basket.FindAsync(id);
            db.Basket.Remove(basket);
            await db.SaveChangesAsync();
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
            public async Task<ActionResult> AddProduct(int id)
        {
            //var principal = Thread.CurrentPrincipal;
            //var userId = principal.Identity.GetUserId();
            string userName = HttpContext.User.Identity.Name;
            //А корзина точно есть?
            var basket = db.Users.Where(i => i.UserName == userName).Select(b=>b.Basket).FirstOrDefault();
            // Product p = (Product)db.Products.Where(i => i.Id == id).FirstOrDefault();
            

            Product prod2 = new Product();
            prod2.Description = "htyrjkrty";
            prod2.Count = 22;
            prod2.Price = 2352;
            prod2.Name = ",yu234tyier235gr";

            basket.Products.Add(prod2);
            List<CartLine> c = new List<CartLine>();
            foreach (var i in basket.Products) {
                int temp = 0;
                foreach (var item in basket.Products)
            var line = basket.lineCollection.Where(p => p.Product.Id == id).FirstOrDefault();
            

            //Basket basket = (Basket)db.Users.Where(i => i.UserName == userName).Select(b=>b.Basket).FirstOrDefault();

            if (line == null)
            {
                basket.lineCollection.Add(new CartLine
                {
                    if (i.Id == item.Id)
                        temp++;
                }
                CartLine t = new CartLine { Product = i, Quantity = temp };
                c.Add(t);
            }
            db.SaveChanges();
            ViewBag.List = c;
            return View();
        }
    }
}
