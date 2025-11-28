using Microsoft.EntityFrameworkCore;
using BaitapcuoikyBE.Models;

namespace BaitapcuoikyBE.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderDetail>()
            .HasKey(od => new { od.OrderId, od.ProductId });

        // FK relations (optional)
        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Order)
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(od => od.OrderId);

        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Product)
            .WithMany()
            .HasForeignKey(od => od.ProductId);
    }
}

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        // seed admin user
        if (!db.Users.Any(u => u.Email == "admin@shop.test"))
        {
            db.Users.Add(new User
            {
                Email = "admin@shop.test",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = "Admin"
            });
        }

        if (!db.Users.Any(u => u.Email == "user@shop.test"))
        {
            db.Users.Add(new User
            {
                Email = "user@shop.test",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),
                Role = "User"
            });
        }

        if (!db.Products.Any())
        {
            db.Products.AddRange(
                new Product { Name = "Bút bi", Price = 10000, Description = "Bút", Stock = 100 },
                new Product { Name = "Vở 100 trang", Price = 20000, Description = "Vở", Stock = 50 }
            );
        }

        db.SaveChanges();
    }
}
