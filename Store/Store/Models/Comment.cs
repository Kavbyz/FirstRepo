﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public string Commen_Name { get; set; }
        public string Comment_Text { get; set; }
        public DateTime Date { get; set; }
        public Product Product { get; set; }
    }
}