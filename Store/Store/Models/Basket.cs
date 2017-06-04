using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.Models
{
    //public class Basket
    //{
    //    private List<CartLine> lineCollection = new List<CartLine>();
    //    public int Id { get; set; }
    //    public virtual ICollection<Product> Products { get; set; }
    //    public Basket()
    //    {
    //        Products = new List<Product>();
    //    }
    //    public IEnumerable<CartLine> Lines
    //    {
    //        get { return lineCollection; }
    //    }
    //}
    public class Basket
    {
        public int Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Count> CountProduct { get; set; }
        public Basket()
        {
            Products = new List<Product>();
        }
    }

    public class CartLine
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}


//public void AddItem(Product product, int quantity)//добавление в корзину продукта определенного количества
//{
//    CartLine line = lineCollection.Where(p => p.Product.Id == product.Id).FirstOrDefault();

//    if (line == null)
//    {
//        lineCollection.Add(new CartLine
//        {
//            Product = product,
//            Quantity = quantity
//        }
//            );
//    }
//    else
//    {
//        line.Quantity += quantity;
//    }
//}

//public void RemoveLine(Product product)
//{
//    lineCollection.RemoveAll(l => l.Product.Id == product.Id);
//}

//public decimal ComputeTotalValue()
//{
//    return lineCollection.Sum(e => e.Product.Price * e.Quantity);
//}

//public void Clear()
//{
//    lineCollection.Clear();
//}

//public IEnumerable<CartLine> Lines
//{
//    get { return lineCollection; }
//}
//    }