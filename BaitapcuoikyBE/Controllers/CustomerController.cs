using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BaitapcuoikyBE.Data;
using BaitapcuoikyBE.Models;

namespace BaitapcuoikyBE.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly AppDbContext _db;
    public CustomersController(AppDbContext db) => _db = db;

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult GetAll() => Ok(_db.Customers.ToList());

    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var c = _db.Customers.Find(id);
        if (c == null) return NotFound();
        return Ok(c);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult Create([FromBody] Customer dto)
    {
        _db.Customers.Add(dto);
        _db.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Customer dto)
    {
        var c = _db.Customers.Find(id);
        if (c == null) return NotFound();
        c.Name = dto.Name; c.Email = dto.Email; c.Phone = dto.Phone; c.Address = dto.Address;
        _db.SaveChanges();
        return Ok(c);
    }

    [Authorize(Roles = "Admin")]
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
