using Microsoft.EntityFrameworkCore;
using SAOnlineMart.API.Models;

using System.Net;

public class SAOnlineMartContext : DbContext
{
    public SAOnlineMartContext(DbContextOptions<SAOnlineMartContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    // You can add more DbSet properties if you add more entities to your project
}
