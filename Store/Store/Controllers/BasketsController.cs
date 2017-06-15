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
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Store.Controllers
{
    public class BasketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult ViewBasket(int idBasket, string message)
        {
            Basket basket = db.Basket.Find(idBasket);
            ViewBag.Message = message;
            int total = 0;
            foreach(var item in basket.CountProduct)
            {
                total += item.CountProduct * item.Product.Price;
            }
            ViewBag.Total = total;
            ViewBag.idBasket = idBasket;
            return View("AddProduct", basket.CountProduct.ToList());
        }
        public async Task<ActionResult> AddProduct(int? id)
        {
            Basket basket=null;
            String userId = User.Identity.GetUserId();
            if (User.Identity.IsAuthenticated == false)
            {
                if (HttpContext.Request.Cookies["Basket"] == null)
                {
                    basket = new Basket();
                    db.Basket.Add(basket);
                    db.SaveChanges();
                    HttpContext.Response.Cookies["Basket"].Value = basket.Id.ToString();
                    HttpContext.Response.Cookies["Basket"].Expires = DateTime.Now.AddDays(10);
                }
                else
                {
                    String basketId = HttpContext.Request.Cookies["Basket"].Value.ToString();
                    basket = db.Basket.Where(b => b.Id.ToString() == basketId).FirstOrDefault();
                }
            }
            else
            {

                if (db.Basket.Where(a => a.User.Id == userId).FirstOrDefault() != null)
                {
                    basket = db.Basket.Where(a => a.User.Id == userId).FirstOrDefault();
                }
                else
                {
                    if (HttpContext.Request.Cookies["Basket"] == null)
                    {
                        basket = new Basket();                        
                        basket.User = db.Users.Where(u => u.Id == userId).FirstOrDefault();
                        db.Basket.Add(basket);
                        HttpContext.Response.Cookies["Basket"].Value = basket.Id.ToString();
                        HttpContext.Response.Cookies["Basket"].Expires = DateTime.Now.AddDays(10);
                    }
                    else
                    {
                        String basketId = HttpContext.Request.Cookies["Basket"].Value.ToString();
                        basket = db.Basket.Where(b => b.Id.ToString() == basketId).FirstOrDefault();
                        basket.User = db.Users.Where(u => u.Id == userId).FirstOrDefault();
                        db.Entry(basket).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
             
            if (basket!=null)
            {
                Product p = db.Products.Where(i => i.Id == id).FirstOrDefault();                
                if (basket.CountProduct.Where(i => i.Product.Id == id).ToList().Count>0)
                {
                    if (basket.CountProduct.Where(i => i.Product.Id == id).FirstOrDefault().CountProduct >= db.Products.Where(i => i.Id == id).FirstOrDefault().Count)
                    {
                        //ModelState.AddModelError("CountProduct", "Больше товаров добавить нельзя");
                        return RedirectToAction("ViewBasket", new { idBasket = basket.Id, message= "Этого товара больше добавить нельзя" });
                    }
                    basket.CountProduct.Where(i => i.Product.Id == id).FirstOrDefault().CountProduct++;

                    db.Entry(basket.CountProduct.Where(i => i.Product.Id == id).FirstOrDefault()).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else if (basket.CountProduct.Where(i => i.Product.Id == id).ToList().Count==0)
                {
                    Count count = new Count();
                    count.CountProduct = 1;
                    count.Product = p;
                    db.Count.Add(count);
                    db.SaveChanges();
                    basket.CountProduct.Add(count);

                    db.SaveChanges();
                }
            }            
            return RedirectToAction("ViewBasket", new { idBasket=basket.Id, message = "Товар успешно добавлен в корзину" });
        }

        [HttpPost]
        public ActionResult AddCount()
        {
            var current = Int32.Parse(Request.Form.GetValues("count").FirstOrDefault().ToString());
            var idCount= Int32.Parse(Request.Form.GetValues("idCount").FirstOrDefault().ToString());
            Count count = db.Count.Find(idCount);
            count.CountProduct = current;
            db.Entry(count).State = EntityState.Modified;
            db.SaveChanges();
            Basket basket = count.Basket;
            int total = 0;
            foreach (var item in basket.CountProduct)
            {
                total += item.CountProduct * item.Product.Price;
            }
            ViewBag.Total = total;
            return PartialView(basket.CountProduct.ToList());
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
            
            Count count = db.Count.Find(id);
            Basket basket = count.Basket;

            basket.CountProduct.Remove(count);
            db.SaveChanges();
            db.Count.Remove(count);
            db.SaveChanges();
            return PartialView("AddCount", basket.CountProduct.ToList());
        }
        //public async Task<ActionResult> Order()
        //{
        //    string userName = HttpContext.User.Identity.Name;
        //    var user = db.Users.Where(i => i.UserName == userName).FirstOrDefault();
        //    var basket = db.Basket.Where(b => b.User.Id == user.Id).FirstOrDefault();
        //    db.Orders.Add(new Order { Time = DateTime.Now, Status ="хз",User=user, Products = basket.CountProduct});

        //  //  basket.Products = new List<Product>();
        //    basket.CountProduct = new List<Count>();
        //    db.SaveChanges();

        //    return RedirectToAction("Index","Home");
        //}
    }
}
