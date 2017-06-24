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
using Store.Filters;

namespace Store.Controllers
{
    [Authorize(Roles = "Admin")]
    [Culture]
    public class HeadingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Headings
        public async Task<ActionResult> Index()
        {
            HttpContext.Response.Cookies["Basket"].Expires = DateTime.Now.AddDays(-1);
            ViewBag.HeadingsList = db.Headings.ToList();
            return View(await db.Headings.ToListAsync());
        }

        // GET: Headings/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Headings headings = await db.Headings.FindAsync(id);
            if (headings == null)
            {
                return HttpNotFound();
            }
            return View(headings);
        }

        // GET: Headings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Headings/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] Headings headings)
        {
            if (ModelState.IsValid)
            {
                db.Headings.Add(headings);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(headings);
        }

        // GET: Headings/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Headings headings = await db.Headings.FindAsync(id);
            if (headings == null)
            {
                return HttpNotFound();
            }
            return View(headings);
        }

        // POST: Headings/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] Headings headings)
        {
            if (ModelState.IsValid)
            {
                db.Entry(headings).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(headings);
        }

        // GET: Headings/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Headings headings = await db.Headings.FindAsync(id);
            if (headings == null)
            {
                return HttpNotFound();
            }
            return View(headings);
        }

        // POST: Headings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Headings headings = await db.Headings.FindAsync(id);
            db.Headings.Remove(headings);
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

        [AllowAnonymous]
        public async Task<ActionResult> SearchProducts(int? id, int temp = 0, string ProdName = null)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            ViewBag.HeadingsList = db.Headings.ToList();
            var M = db.Products.ToList();

            if (id != -1)
            {
                M = db.Headings.Where(h => h.Id == id).SelectMany(p => p.Products).ToList();
            }
            ViewBag.ProdName = null;
            if (ProdName != null)
            {
                    M = db.Products.Where(i => i.Name.Contains(ProdName)).ToList();
                    ViewBag.ProdName = ProdName;
            }

            ViewBag.HeadingId = id;
            if (temp == 1)
            {
                M = M.OrderByDescending(t => t.Price).ToList();
            }
            if (temp == 2)
            {
                M = M.OrderBy(t => t.Price).ToList();
            }
            //if (temp == 3)
            //{
            //    var M = db.Headings.Where(h => h.Id == id).SelectMany(p => p.Products).ToList().OrderBy(t => t.Price); // По популярности
            //    return View(M);
            //}
            ViewBag.HeadingsList = db.Headings.ToList();
            return View(M);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ShowProduct(int? id)
        {
            ViewBag.HeadingsList = db.Headings.ToList();
            return View("SearchProducts", await db.Products.Where(i => i.Id == id).ToListAsync());
        }
    }
}
