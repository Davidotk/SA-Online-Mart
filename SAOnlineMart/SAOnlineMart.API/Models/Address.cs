namespace SAOnlineMart.API.Models
{
    public class Address
    {
        public int AddressID { get; set; }
        public int UserID { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }
        public string? Suburb { get; set; }
        public string? Country { get; set; }

        // Navigation properties
        public User User { get; set; }
    }
}
