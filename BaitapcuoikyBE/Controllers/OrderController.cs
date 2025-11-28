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

    // 1. Tạo đơn hàng (User) - Code cũ đã có, giữ nguyên logic
    [Authorize(Roles = "User")]
    [HttpPost]
    public IActionResult Create([FromBody] OrderCreateDto dto)
    {
        // ... (Logic tạo đơn hàng như đã sửa ở bước trước) ...
        // Để ngắn gọn, tôi không paste lại đoạn Create dài dòng ở đây
        // Hãy giữ nguyên hàm Create mà bạn đã sửa để fix lỗi Foreign Key
        // Dưới đây là đoạn code Create tóm tắt:
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        // ... Logic kiểm tra tồn kho ...
        // ... Logic lưu Order và OrderDetails ...
        // _db.SaveChanges();
        return Ok(new { msg = "Thành công" });
    }

    // 2. [MỚI] Admin xem toàn bộ đơn hàng + Chi tiết
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult GetAll()
    {
        var list = _db.Orders
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product) // Kèm thông tin sản phẩm
            .OrderByDescending(o => o.CreatedAt)
            .ToList();
        return Ok(list);
    }

    // 3. [MỚI] User xem lịch sử đơn hàng của mình
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