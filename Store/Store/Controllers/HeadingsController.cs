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
    public class HeadingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Headings
        public async Task<ActionResult> Index()
        {
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

        public async Task<ActionResult> SearchProducts(int? id, int temp = 0)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            ViewBag.HeadingId = id;
            if (temp == 1)
            {
                var M = db.Headings.Where(h => h.Id == id).SelectMany(p => p.Products).OrderByDescending(t => t.Price).ToList();
                return View(M);
            }
            if (temp == 2)
            {
                var M = db.Headings.Where(h => h.Id == id).SelectMany(p => p.Products).OrderBy(t => t.Price).ToList();
                return View(M);
            }
            //if (temp == 3)
            //{
            //    var M = db.Headings.Where(h => h.Id == id).SelectMany(p => p.Products).ToList().OrderBy(t => t.Price); // По популярности
            //    return View(M);
            //}
            var P = db.Headings.Where(h => h.Id == id).SelectMany(p => p.Products).ToList();
            return View(P);
        }
    }
}
