using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.Models
{
    public class Product
    {
        public Product()
        {
            Headings = new List<Headings>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Headings> Headings { get; set; }
        //List image        
    }
}