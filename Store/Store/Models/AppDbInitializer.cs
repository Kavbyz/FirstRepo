using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Store.Models
{
    public class AppDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            Headings heading = new Headings();
            heading.Name = "First";
            context.Headings.Add(heading);
            context.SaveChanges();

            Headings heading1 = new Headings();
            heading1.Name = "Double";
            context.Headings.Add(heading1);
            context.SaveChanges();

            Product prod = new Product();
            prod.Description = "gwergweg";
            prod.Count = 2;
            prod.Price = 1500;
            prod.Name = "fweww21312";
            List<Headings> h = new List<Headings>();
            h.Add(heading);
            prod.Headings = h;

            Product prod2 = new Product();
            prod2.Description = "htyrjkrty";
            prod2.Count = 22;
            prod2.Price = 2352;
            prod2.Name = ",yu234tyier235gr";
            prod2.Headings = h;

            context.Products.Add(prod);
            context.Products.Add(prod2);
            context.SaveChanges();


            base.Seed(context);
        }
    }
}