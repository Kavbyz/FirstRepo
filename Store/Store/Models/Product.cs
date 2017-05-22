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

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        
        //List image
    }
}