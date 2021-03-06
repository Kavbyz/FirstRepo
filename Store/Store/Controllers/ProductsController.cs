﻿using System;
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
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        public ActionResult SendComment(string NameUser, string CommentUser, int IdProduct)
        {
            Store.Models.Comment comment = new Models.Comment();
            comment.Comment_Text = CommentUser;
            comment.Commen_Name = NameUser;
            comment.Product = db.Products.Where(a => a.Id == IdProduct).FirstOrDefault();
            comment.Date = DateTime.Now;

            db.Comments.Add(comment);
            db.SaveChanges();

            return PartialView(db.Comments.Where(a=>a.Product.Id==IdProduct).ToList());
        }

        public ActionResult DeleteComment(int id)
        {
            Store.Models.Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();

            return PartialView("SendComment", db.Comments.ToList());
        }



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
        [AllowAnonymous]
        public async Task<ActionResult> Details(int? id)
        {
            ViewBag.HeadingsList = db.Headings.ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            ViewBag.Comment = db.Comments.Where(comment => comment.Product.Id == product.Id).ToList();            
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
            ViewBag.HeadingsListProduct = HeadingsName;
            ViewBag.HeadingsList = db.Headings.ToList();
            //ViewBag.Headings = new SelectList(HeadingsName);
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
                db.Products.Add(product);
                await db.SaveChangesAsync();
                if (Heading != null)
                {
                    foreach (var item in Heading)
                    {
                        Headings Headings = db.Headings.ToList().Where(a => a.Name == item).FirstOrDefault();
                        product.Headings.Add(Headings);
                    }
                    await db.SaveChangesAsync();
                }
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

                return RedirectToAction("Index", "Home");
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
            List<string> HeadingsName = new List<string>();
            foreach (var item in db.Headings.ToList())
            {
                HeadingsName.Add(item.Name);
            }
            ViewBag.HeadingsListProduct = HeadingsName;
            ViewBag.Headings = new SelectList(HeadingsName);
            ViewBag.HeadingsList = db.Headings.ToList();
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Count,Price,Description")] Product product, List<string> Img, List<string> Heading)
        {
            if (ModelState.IsValid)
            {
                Product productTrouble = await db.Products.FindAsync(product.Id);
                productTrouble.Name = product.Name;
                productTrouble.Count = product.Count;
                productTrouble.Price = product.Price;
                productTrouble.Description = product.Description;


                foreach (var item in productTrouble.Headings.ToList())
                {
                    productTrouble.Headings.Remove(item);
                }
                if (Heading != null)
                {
                    foreach (var item in Heading)
                    {
                        Headings Headings = db.Headings.ToList().Where(a => a.Name == item).FirstOrDefault();
                        productTrouble.Headings.Add(Headings);
                    }
                }
                

                db.Entry(productTrouble).State = EntityState.Modified;
                await db.SaveChangesAsync();

                if (db.Images.ToList().Count > 0)
                {
                    foreach (var item in db.Images.ToList())
                    {
                        if (item.Product.Id == productTrouble.Id)
                        {
                            db.Images.Remove(item);
                        }
                    }
                    await db.SaveChangesAsync();
                }

                if (Img != null)
                {
                    foreach (var item in Img)
                    {
                        Images img = new Images();
                        img.Path = item;
                        img.Product = productTrouble;
                        db.Images.Add(img);
                    }
                    await db.SaveChangesAsync();
                }


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
            if (db.Images.ToList().Count > 0)
            {
                foreach (var item in db.Images.ToList())
                {
                    if (item.Product.Id == product.Id)
                    {
                        db.Images.Remove(item);
                    }
                }
                await db.SaveChangesAsync();
            }
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
