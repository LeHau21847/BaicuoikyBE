using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BaitapcuoikyBE.Data;
using BaitapcuoikyBE.Models;

namespace BaitapcuoikyBE.Controllers;

[ApiController]
[Route("api/customers")]
[Authorize(Roles = "Admin")] // Chỉ Admin mới vào được
public class CustomersController : ControllerBase
{
    private readonly AppDbContext _db;
    public CustomersController(AppDbContext db) => _db = db;

    // Xem danh sách
    [HttpGet]
    public IActionResult GetAll() => Ok(_db.Customers.ToList());

    // Xem chi tiết
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var c = _db.Customers.Find(id);
        return c == null ? NotFound() : Ok(c);
    }

    // Thêm mới
    [HttpPost]
    public IActionResult Create([FromBody] Customer dto)
    {
        if (_db.Customers.Any(c => c.Email == dto.Email)) 
            return BadRequest("Email đã tồn tại!");
        
        _db.Customers.Add(dto);
        _db.SaveChanges();
        return Ok(dto);
    }

    // Cập nhật
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Customer dto)
    {
        var c = _db.Customers.Find(id);
        if (c == null) return NotFound();

        c.Name = dto.Name;
        c.Phone = dto.Phone;
        c.Address = dto.Address;
        // Không cho sửa Email để tránh lỗi dữ liệu
        
        _db.SaveChanges();
        return Ok(c);
    }

    // Xóa
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var c = _db.Customers.Find(id);
        if (c == null) return NotFound();
        _db.Customers.Remove(c);
        _db.SaveChanges();
        return NoContent();
    }
}