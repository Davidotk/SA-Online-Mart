using System.Collections.Generic;

namespace SAOnlineMart.API.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        // Navigation properties
        public ICollection<Product> Products { get; set; }
    }
}
