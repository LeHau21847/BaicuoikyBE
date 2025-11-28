using Microsoft.EntityFrameworkCore;
using BaitapcuoikyBE.Models;
using BCrypt.Net;

namespace BaitapcuoikyBE.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

    // Các DbSet (Bảng)
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1. Khóa tổng hợp (Composite Key) cho OrderDetail
        modelBuilder.Entity<OrderDetail>().HasKey(od => new { od.OrderId, od.ProductId });
        
        // 2. Định nghĩa các mối quan hệ (cho rõ ràng)
        
        // Order <-> Customer (1-n)
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany()
            .HasForeignKey(o => o.CustomerId);

        // OrderDetail <-> Order (n-1)
        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Order)
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(od => od.OrderId);

        // OrderDetail <-> Product (n-1)
        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Product)
            .WithMany()
            .HasForeignKey(od => od.ProductId);
    }
}

// -----------------------------------------------------------
// Lớp Gieo hạt dữ liệu (Data Seeder)
// -----------------------------------------------------------

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        // 1. Tạo User Admin
        if (!db.Users.Any(u => u.Email == "admin@shop.test"))
        {
            db.Users.Add(new User { Email = "admin@shop.test", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"), Role = "Admin" });
            db.SaveChanges(); // Lưu để đảm bảo có ID cho các bước sau
        }

        // 2. Tạo User thường và Customer (FIX LỖI FOREIGN KEY và SQL INJECTION)
        if (!db.Users.Any(u => u.Email == "user@shop.test"))
        {
            var user = new User { Email = "user@shop.test", PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"), Role = "User" };
            db.Users.Add(user);
            db.SaveChanges(); // LƯU User để lấy user.Id

            // Dữ liệu Customer cần insert.
            // Do Customer Id và User Id phải trùng nhau để phục vụ Auth/Order
            var customer = new Customer 
            { 
                Name = "Khách User Mẫu", 
                Email = "user@shop.test", 
                Phone = "0123456789", 
                Address = "Địa chỉ mẫu" 
            };
            
            // Sử dụng ExecuteSqlRaw với tham số để chèn Customer với ID = user.Id
            // CÁCH NÀY TRÁNH LỖI SQL INJECTION
            db.Database.ExecuteSqlRaw(
                "INSERT INTO Customers (Id, Name, Email, Phone, Address) VALUES ({0}, {1}, {2}, {3}, {4})",
                user.Id, // {0}: ID lấy từ User vừa tạo
                customer.Name, // {1}
                customer.Email, // {2}
                customer.Phone, // {3}
                customer.Address // {4}
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