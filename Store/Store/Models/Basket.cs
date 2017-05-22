using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.Models
{
    public class Basket
    {
        public int Id { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public Basket()
        {
            Products = new List<Product>();
        }
    }
}