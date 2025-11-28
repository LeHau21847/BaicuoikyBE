using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BaitapcuoikyBE.Data;
using BaitapcuoikyBE.Dtos;
using BaitapcuoikyBE.Models;

namespace BaitapcuoikyBE.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _db;
    public OrdersController(AppDbContext db) => _db = db;

    // 1. Tạo đơn hàng (User) - Logic hoàn chỉnh
    [Authorize(Roles = "User")]
    [HttpPost]
    public IActionResult Create([FromBody] OrderCreateDto dto)
    {
        if (dto.Items == null || dto.Items.Count == 0) return BadRequest("Giỏ hàng trống!");

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        // Kiểm tra bảo mật: User chỉ được đặt hàng cho chính mình
        if (dto.CustomerId != userId) return Forbid(); 

        decimal total = 0m;
        var details = new List<OrderDetail>();

        foreach (var item in dto.Items)
        {
            if (item.Quantity <= 0) return BadRequest("Số lượng phải lớn hơn 0");

            var product = _db.Products.Find(item.ProductId);
            
            // Kiểm tra tồn tại và tồn kho
            if (product == null) return BadRequest($"Sản phẩm ID {item.ProductId} không tồn tại");
            if (product.Stock < item.Quantity) return BadRequest($"Sản phẩm {product.Name} không đủ hàng (Còn: {product.Stock})");

            // Tạo chi tiết đơn
            details.Add(new OrderDetail 
            { 
                ProductId = item.ProductId, 
                Quantity = item.Quantity, 
                UnitPrice = product.Price 
            });

            // Cập nhật tổng tiền và trừ kho
            total += product.Price * item.Quantity;
            product.Stock -= item.Quantity; 
        }

        var order = new Order 
        { 
            CustomerId = userId, 
            TotalPrice = total, 
            Status = "Pending",
            CreatedAt = DateTime.UtcNow,
            OrderDetails = details 
        };

        _db.Orders.Add(order);
        _db.SaveChanges(); // Lưu Order và OrderDetails cùng với việc cập nhật Stock

        // Trả về kết quả thành công
        return Ok(new { msg = "Đặt hàng thành công", orderId = order.Id });
    }

    // 2. Admin xem toàn bộ đơn hàng + Chi tiết
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult GetAll()
    {
        var list = _db.Orders
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product) 
            .OrderByDescending(o => o.CreatedAt)
            .ToList();
        return Ok(list);
    }

    // 3. User xem lịch sử đơn hàng của mình
    [Authorize(Roles = "User")]
    [HttpGet("my-orders")]
    public IActionResult GetMyOrders()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var list = _db.Orders
            .Where(o => o.CustomerId == userId)
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product)
            .OrderByDescending(o => o.CreatedAt)
            .ToList();
        return Ok(list);
    }
}