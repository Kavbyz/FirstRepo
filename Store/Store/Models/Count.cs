﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.Models
{
    public class Count
    {
        public int Id { get; set; }
        public int IdProduct { get; set; }
        public int CountProduct { get; set; }
        public virtual Basket Basket{ get; set; }
}
}