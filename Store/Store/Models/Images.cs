using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.Models
{
    public class Images
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public virtual Product Product { get; set; }
    }
}