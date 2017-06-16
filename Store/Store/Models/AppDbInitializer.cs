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
            heading.Name = "Первая";
            context.Headings.Add(heading);
            context.SaveChanges();

            Headings heading1 = new Headings();
            heading1.Name = "Вторая";
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
            List<Headings> h1 = new List<Headings>();
            h1.Add(heading1);
            prod.Headings = h;

            Product prod2 = new Product();
            prod2.Description = "htyrjhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtyhtyrjkrtykrty";
            prod2.Count = 22;
            prod2.Price = 2352;
            prod2.Name = ",yu234tyier235gr";
            prod2.Headings = h;

            Product prod3 = new Product();
            prod3.Description = "jtyjtyjy";
            prod3.Count = 22;
            prod3.Price = 2352;
            prod3.Name = ",,gh,jkghuyk";
            prod3.Headings = h1;

            Product prod4 = new Product();
            prod4.Description = "htyrjkrty";
            prod4.Count = 22;
            prod4.Price = 2352;
            prod4.Name = ",cvxcsdcs";
            prod4.Headings = h;

            Product prod5 = new Product();
            prod5.Description = "htyrjkrty";
            prod5.Count = 22;
            prod5.Price = 2352;
            prod5.Name = ",2346346346";
            prod5.Headings = h1;


            context.Products.Add(prod);
            context.Products.Add(prod2);
            context.Products.Add(prod3);
            context.Products.Add(prod4);
            context.Products.Add(prod5);
            context.SaveChanges();

            Images img = new Images();
            img.Path = "/Images/noavatar.png";
            img.Product=prod;
            context.Images.Add(img);
            context.SaveChanges();

            Images img1 = new Images();
            img1.Path = "/Images/Lazy.jpg";
            img1.Product = prod2;
            context.Images.Add(img1);
            context.SaveChanges();

            Images img2 = new Images();
            img.Path = "/Images/noavatar.png";
            img.Product = prod3;
            context.Images.Add(img);
            context.SaveChanges();

            Images img3 = new Images();
            img.Path = "/Images/noavatar.png";
            img.Product = prod4;
            context.Images.Add(img);
            context.SaveChanges();

            Images img4 = new Images();
            img.Path = "/Images/noavatar.png";
            img.Product = prod5;
            context.Images.Add(img);
            context.SaveChanges();

            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context));

            var role1 = new ApplicationRole { Name = "Admin" };
            var role2 = new ApplicationRole { Name = "User" };

            roleManager.Create(role1);
            roleManager.Create(role2);

            var admin = new ApplicationUser { Email = "somemail@mail.ru", UserName = "somemail@mail.ru" };
            string password = "ad46D_ewr3";
            var result = userManager.Create(admin, password);

            // если создание пользователя прошло успешно
            if (result.Succeeded)
            {
                // добавляем для пользователя роль
                userManager.AddToRole(admin.Id, role1.Name);
                userManager.AddToRole(admin.Id, role2.Name);
            }

            Order order = new Order();
            order.Adress = "123";
            order.Comment = "123";
            order.Delivery = "123";
            order.Email = "123";
            order.Name = "123";
            order.Status = "Новый";
            order.SurName = "123";
            order.Telephone = "123";
            order.Time = DateTime.Now;
            context.Orders.Add(order);
            context.SaveChanges();

            Count count = new Count();
            count.CountProduct = 1;
            count.Product = prod;
            count.Order = order;
            context.Count.Add(count);
            context.SaveChanges();

            

            //Comment comment = new Comment();
            //comment.Commen_Name = "qwerty";
            //comment.Comment_Text = "asdfgh";
            //comment.Product = prod;
            //comment.Date = DateTime.Now;
            //context.Comments.Add(comment);
            //context.SaveChanges();

            //Basket basket = new Basket();
            //basket.User = admin;
            //context.Basket.Add(basket);
            //context.SaveChanges();

            base.Seed(context);
        }
    }
}