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
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        public JsonResult Upload()
        {
            List<string> path = new List<string>();
            foreach (string file in Request.Files)
            {                
                var upload = Request.Files[file];
                if (upload != null)
                {
                    // получаем имя файла
                    string fileName = System.IO.Path.GetFileName(upload.FileName);
                    fileName = Guid.NewGuid().ToString() + fileName;                    

                    // сохраняем файл в папку Files в проекте
                    upload.SaveAs(Server.MapPath("~/Images/" + fileName));

                    path.Add("/Images/" + fileName);
                }
            }
            return Json(path);
        }

        [HttpPost]
        public JsonResult DeleteImageOnCreate()
        {
            for(int i=0; i<Int32.Parse(Request.Form.GetValues("imglenght").FirstOrDefault());i++)
            {
                String path = Server.MapPath("~"+ Request.Form.GetValues("img"+i).FirstOrDefault().ToString());
                System.IO.File.Delete(path);
            }
            return Json("Success");
        }

        // GET: Products
        public async Task<ActionResult> Index()
        {
            ViewBag.HeadingsList = db.Headings.ToList();
            return View(await db.Products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            List<string> HeadingsName = new List<string>();
            foreach(var item in db.Headings.ToList())
            {
                HeadingsName.Add(item.Name);
            }
            ViewBag.Headings = new SelectList(HeadingsName);
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Count,Price,Description")] Product product, List<string> Img, List<string> Heading)
        {
            if (ModelState.IsValid)
            {
                if (Heading != null)
                {
                    foreach (var item in Heading)
                    {
                        Headings Headings = db.Headings.ToList().Where(a => a.Name == item).FirstOrDefault();
                        product.Headings.Add(Headings);
                    }
                    await db.SaveChangesAsync();
                }
                db.Products.Add(product);
                await db.SaveChangesAsync();                
                if (Img != null)
                {
                    foreach (var item in Img)
                    {
                        Images img = new Images();
                        img.Path = item;
                        img.Product = product;
                        db.Images.Add(img);
                    }
                    await db.SaveChangesAsync();
                }
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Count,Price,Description")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
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
