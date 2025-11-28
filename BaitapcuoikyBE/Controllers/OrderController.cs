using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BaitapcuoikyBE.Data;
using BaitapcuoikyBE.Dtos;
using BaitapcuoikyBE.Models;

namespace BaitapcuoikyBE.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _db;
    public OrdersController(AppDbContext db) => _db = db;

    [Authorize]
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var order = _db.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Product).SingleOrDefault(o => o.Id == id);
        if (order == null) return NotFound();

        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        if (role != "Admin" && order.CustomerId != userId) return Forbid();

        return Ok(order);
    }

    [Authorize(Roles = "User")]
    [HttpPost]
    public IActionResult Create([FromBody] OrderCreateDto dto)
    {
        if (dto.Items == null || dto.Items.Count == 0) return BadRequest("Empty order");
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        if (dto.CustomerId != userId) return Forbid();

        decimal total = 0m;
        var details = new List<OrderDetail>();

        foreach (var it in dto.Items)
        {
            if (it.Quantity <= 0) return BadRequest("Quantity > 0 required");
            var prod = _db.Products.Find(it.ProductId);
            if (prod == null) return BadRequest("Invalid product");
            if (prod.Stock < it.Quantity) return BadRequest("Not enough stock");

            details.Add(new OrderDetail { OrderId = 0, ProductId = it.ProductId, Quantity = it.Quantity, UnitPrice = prod.Price });
            total += prod.Price * it.Quantity;
            prod.Stock -= it.Quantity;
        }

        var order = new Order { CustomerId = userId, TotalPrice = total, OrderDetails = details };
        _db.Orders.Add(order);
        _db.SaveChanges();

        return CreatedAtAction(nameof(Get), new { id = order.Id }, new { order.Id, order.TotalPrice });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult GetAll()
    {
        var list = _db.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Product).ToList();
        return Ok(list);
    }

    [Authorize(Roles = "User")]
    [HttpGet("my-orders")]
    public IActionResult MyOrders()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var list = _db.Orders.Where(o => o.CustomerId == userId).Include(o => o.OrderDetails).ThenInclude(od => od.Product).ToList();
        return Ok(list);
    }
}
