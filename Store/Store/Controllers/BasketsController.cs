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

        public async Task<ActionResult> AddProduct(int? id)
        {
            string userName = HttpContext.User.Identity.Name;
            //А корзина точно есть?
            var user = db.Users.Where(i => i.UserName == userName).FirstOrDefault();

            var basket = db.Basket.Where(b => b.User.Id == user.Id).FirstOrDefault();
            
            Product p = (Product)db.Products.Where(i => i.Id == id).FirstOrDefault();
            if (basket.CountProduct.Where(a => a.IdProduct == id).ToList().Count > 0 && basket.CountProduct.Where(i => i.IdProduct == id).First().CountProduct < db.Products.Where(i => i.Id == id).First().Count)
            {
                basket.CountProduct.Where(a => a.IdProduct == id).FirstOrDefault().CountProduct++;
                
                db.Entry(basket.CountProduct.Where(a => a.IdProduct == id).FirstOrDefault()).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                Count count = new Count();
                count.CountProduct = 1;
                count.IdProduct = (int)id;
                count.Basket = basket;

               // basket.Products.Add(p);
                basket.CountProduct.Add(count);

                db.SaveChanges();
            }

            List<CartLine> list = new List<CartLine>();
            int temp = 0;
            foreach (var item in basket.CountProduct)
            {
                temp += db.Products.Where(i => i.Id == item.IdProduct).FirstOrDefault().Price * item.CountProduct;
                list.Add(new CartLine { Product = db.Products.Where(i => i.Id == item.Id).FirstOrDefault(), Quantity = item.CountProduct });
            }
            ViewBag.List = list;
            ViewBag.Price = temp;
            return View();
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

        public async Task<ActionResult> DeleteProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string userName = HttpContext.User.Identity.Name;
            //А корзина точно есть?
            var user = db.Users.Where(i => i.UserName == userName).FirstOrDefault();

            Basket basket = (Basket)db.Basket.Where(b => b.User.Id == user.Id).Include(e=>e.CountProduct).FirstOrDefault();

            Count count = basket.CountProduct.Where(c => c.IdProduct == id).FirstOrDefault();
            //   Product prod = (Product)db.Products.Where(i => i.Id == id).FirstOrDefault();
            basket.CountProduct.Remove(count);
            db.Count.Remove(count);
            //   basket.Products.Remove(prod);
            




            db.SaveChanges();

            List<CartLine> list = new List<CartLine>();
            int temp = 0;
            foreach (var item in basket.CountProduct)
            {
                temp += db.Products.Where(i => i.Id == item.IdProduct).FirstOrDefault().Price * item.CountProduct;
                list.Add(new CartLine { Product = db.Products.Where(i => i.Id == item.Id).FirstOrDefault(), Quantity = item.CountProduct });
            }
            ViewBag.List = list;
            ViewBag.Price = temp;
            return View("AddProduct");
        }
        public async Task<ActionResult> ChengeQuantity(int? id, bool flag)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string userName = HttpContext.User.Identity.Name;
            var user = db.Users.Where(i => i.UserName == userName).FirstOrDefault();

            var basket = db.Basket.Where(b => b.User.Id == user.Id).FirstOrDefault();

            if (flag)
                basket.CountProduct.Where(i => i.IdProduct == id).FirstOrDefault().CountProduct--;
            else if(basket.CountProduct.Where(i => i.IdProduct == id).FirstOrDefault().CountProduct < db.Products.Where(i => i.Id == id).FirstOrDefault().Count)
                basket.CountProduct.Where(i => i.IdProduct == id).FirstOrDefault().CountProduct++;

            if (basket.CountProduct.Where(i => i.IdProduct == id).FirstOrDefault().CountProduct == 0)
                return RedirectToAction("DeleteProduct", "Baskets", new { @id = id });

            db.SaveChanges();
            List<CartLine> list = new List<CartLine>();
            int temp = 0;
            foreach (var item in basket.CountProduct)
            {
                temp += db.Products.Where(i => i.Id == item.IdProduct).FirstOrDefault().Price*item.CountProduct;
                list.Add(new CartLine { Product = db.Products.Where(i => i.Id == item.Id).FirstOrDefault(), Quantity = item.CountProduct });
            }
            ViewBag.List = list;
            ViewBag.Price = temp;
            return View("AddProduct");
        }
        public async Task<ActionResult> Order()
        {
            string userName = HttpContext.User.Identity.Name;
            var user = db.Users.Where(i => i.UserName == userName).FirstOrDefault();
            var basket = db.Basket.Where(b => b.User.Id == user.Id).FirstOrDefault();
            db.Orders.Add(new Order { Time = DateTime.Now, Status ="хз",User=user, Products = basket.CountProduct});

          //  basket.Products = new List<Product>();
            basket.CountProduct = new List<Count>();
            db.SaveChanges();

            return RedirectToAction("Index","Home");
        }
    }
}
