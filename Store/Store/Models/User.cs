using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FIO { get; set; }
        public string Email { get; set; }
        public int Telephone { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}