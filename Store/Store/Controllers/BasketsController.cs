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

namespace Store.Controllers
{
    public class BasketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> AddProduct()
        {
            Basket basket = db.Basket.ToList()[0];
            Product product = db.Products.ToList()[0];
            if (basket.Products.Where(a => a.Id == product.Id).ToList().Count > 0)
            {
                basket.CountProduct.Where(a => a.IdProduct == product.Id).FirstOrDefault().CountProduct++;

                db.Entry(basket.CountProduct.Where(a => a.IdProduct == product.Id).FirstOrDefault()).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                Count count = new Count();
                count.CountProduct = 1;
                count.IdProduct = product.Id;

                basket.Products.Add(product);
                basket.CountProduct.Add(count);

                db.SaveChanges();
            }

            return View(basket.Products.ToList());
        }

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
    }
}
