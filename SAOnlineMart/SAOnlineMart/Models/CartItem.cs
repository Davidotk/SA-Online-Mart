using SAOnlineMart.API.Models;

namespace SAOnlineMart.MVC.Models
{
    public class CartItem
    {
        public Product? Product { get; set; }
        public int Quantity { get; set; }
    }

}
