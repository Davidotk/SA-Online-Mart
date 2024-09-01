using System;
using System.Collections.Generic;

namespace SAOnlineMart.API.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public decimal Total { get; set; }
        public DateTime OrderDate { get; set; }
        public bool isShipped { get; set; }
        public bool isComplete { get; set; }
        public string? ShippingAddress { get; set; }

        // Navigation properties
        public User User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
