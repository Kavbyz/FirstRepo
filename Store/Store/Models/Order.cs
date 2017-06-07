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
        public string Comment { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Adress { get; set; }
        public string Delivery { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Count> Count { get; set; }
    }
}