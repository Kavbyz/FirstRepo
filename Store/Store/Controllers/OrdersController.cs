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
using Microsoft.AspNet.Identity;

namespace Store.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        public async Task<ActionResult> CreateOrder([Bind(Include = "Id,Comment,Name,SurName,Email,Telephone,Adress")] Order order, int idBasket)
        {
            order.Time = DateTime.Now;
            order.Status = "New";
            if(User.Identity.IsAuthenticated)
            {
                order.User = db.Users.Find(User.Identity.GetUserId());
            }
            Basket basket = db.Basket.Find(idBasket);
            order.Count = basket.CountProduct;
            db.Orders.Add(order);
            db.SaveChanges();
            basket.CountProduct.Clear();            
            await db.SaveChangesAsync();
            return View("OrderSuccess");
        }

        [HttpPost]
        public async Task<JsonResult> SearchId()
        {
            int id = Int32.Parse(Request.Form.GetValues("Id").FirstOrDefault().ToString());
            if(db.Orders.Find(id)!=null)
            {
                return Json("Find");
            }
            else
            {
                return Json("NoFind");
            }
            
        }

        public async Task<ActionResult> Orders()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> OrderSearchId(string Id)
        {
            if(db.Orders.Find(Int32.Parse(Id))==null)
            {
                ViewBag.msg = "Такого заказа несуществует!";
                return View("Orders");
            }
            return RedirectToAction("Edit", new { id=Int32.Parse(Id) });
        }

        [HttpPost]
        public async Task<ActionResult> OrderSearchName(string Name, string SurName)
        {
            if (db.Orders.Where(a=>a.Name==Name&&a.SurName==SurName).ToList().Count>0)
            {
                return View("ShowOrders", db.Orders.Where(a => a.Name == Name && a.SurName == SurName).ToList());
            }
            ViewBag.msgName = "Такого заказа несуществует!";
            return View("Orders");
        }


        public async Task<ActionResult> NewOrders()
        {
            return View("ShowOrders", db.Orders.Where(a => a.Status=="New").ToList());
        }

        public async Task<ActionResult> AllOrders()
        {
            return View("ShowOrders", db.Orders.ToList());
        }

        // GET: Orders
        public async Task<ActionResult> Index()
        {
            return View(await db.Orders.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Time,Status,Comment,Name,SurName,Email,Telephone,Adress,Delivery")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Time,Status,Comment,Name,SurName,Email,Telephone,Adress,Delivery")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Order order = await db.Orders.FindAsync(id);
            db.Orders.Remove(order);
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
