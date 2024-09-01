using SAOnlineMart.API.Models;
using System.Collections.Generic;
using System.Linq;

namespace SAOnlineMart.MVC.Models
{
    public class Cart
    {
        private List<CartItem> items = new List<CartItem>();

        public IEnumerable<CartItem> Items => items;

        public void AddItem(Product product, int quantity)
        {
            var item = items.FirstOrDefault(p => p.Product.ProductID == product.ProductID);

            if (item == null)
            {
                items.Add(new CartItem { Product = product, Quantity = quantity });
            }
            else
            {
                item.Quantity += quantity;
            }
        }


        public void RemoveItem(int productId)
        {
            items.RemoveAll(i => i.Product.ProductID == productId);
        }

        public decimal ComputeTotalValue()
        {
            return items.Sum(e => e.Product.Price * e.Quantity);
        }

        public void Clear()
        {
            items.Clear();
        }
    }
}
