using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Status { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Product> product { get; set; }
    }
}