using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Store.Models
{
    public class Product
    {        
        public int Id { get; set; }
        [Display(Name = "NameProduct", ResourceType = typeof(Resources.Resource))]
        public string Name { get; set; }
        [Display(Name = "CountProduct", ResourceType = typeof(Resources.Resource))]
        public int Count { get; set; }
        [Display(Name = "PriceProduct", ResourceType = typeof(Resources.Resource))]
        public int Price { get; set; }
        [Display(Name = "DescriptionProduct", ResourceType = typeof(Resources.Resource))]
        public string Description { get; set; }
        public virtual ICollection<Headings> Headings { get; set; }
        public virtual Order Order { get; set; }
        public virtual ICollection<Images> Images { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public Basket Basket { get; set; }

        public Product()
        {
            Headings = new List<Headings>();
        }
    }
}