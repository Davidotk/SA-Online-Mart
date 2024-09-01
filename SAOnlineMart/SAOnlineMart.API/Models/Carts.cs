using System.Collections.Generic;

namespace SAOnlineMart.API.Models
{
    public class Cart
    {
        public int CartID { get; set; }
        public int UserID { get; set; }

        // Navigation properties
        public User User { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
