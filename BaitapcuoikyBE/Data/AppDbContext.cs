using Microsoft.EntityFrameworkCore;
using BaitapcuoikyBE.Models;
using BCrypt.Net;

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
        modelBuilder.Entity<OrderDetail>().HasKey(od => new { od.OrderId, od.ProductId });
        
        modelBuilder.Entity<Order>().HasOne(o => o.Customer).WithMany().HasForeignKey(o => o.CustomerId);
        modelBuilder.Entity<OrderDetail>().HasOne(od => od.Order).WithMany(o => o.OrderDetails).HasForeignKey(od => od.OrderId);
        modelBuilder.Entity<OrderDetail>().HasOne(od => od.Product).WithMany().HasForeignKey(od => od.ProductId);
    }
}

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        // 1. Tạo User Admin
        if (!db.Users.Any(u => u.Email == "admin@shop.test"))
        {
            db.Users.Add(new User { Email = "admin@shop.test", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"), Role = "Admin" });
            db.SaveChanges(); 
        }

        // 2. Tạo User thường và Customer (FIX LỖI FOREIGN KEY)
        if (!db.Users.Any(u => u.Email == "user@shop.test"))
        {
            var user = new User { Email = "user@shop.test", PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"), Role = "User" };
            db.Users.Add(user);
            db.SaveChanges(); // LƯU User để lấy user.Id

            var customer = new Customer { Name = "Khách User Mẫu", Email = "user@shop.test", Phone = "0123456789", Address = "Địa chỉ mẫu" };
            
            // Dùng SQL Raw với tham số để chèn Customer với ID = user.Id
            db.Database.ExecuteSqlRaw(
                "INSERT INTO Customers (Id, Name, Email, Phone, Address) VALUES ({0}, {1}, {2}, {3}, {4})",
                user.Id, customer.Name, customer.Email, customer.Phone, customer.Address
            );
        }
        
        // 3. Tạo Sản phẩm mẫu
        if (!db.Products.Any())
        {
            db.Products.AddRange(
                new Product { Name = "Bút bi Thiên Long", Price = 5000, Description = "Mực xanh", Stock = 100 },
                new Product { Name = "Vở Campus", Price = 12000, Description = "100 trang", Stock = 50 },
                new Product { Name = "Máy tính Casio", Price = 500000, Description = "FX-580VN", Stock = 10 }
            );
            db.SaveChanges();
        }
    }
}