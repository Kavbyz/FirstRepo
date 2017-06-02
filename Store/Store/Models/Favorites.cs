using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.Models
{
    public class Favorites
    {
        public int Id { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ApplicationUser User { get; set; }
        public Favorites()
        {
            Products = new List<Product>();
        }
    }
}