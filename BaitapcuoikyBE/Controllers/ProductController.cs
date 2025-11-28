using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BaitapcuoikyBE.Data;
using BaitapcuoikyBE.Dtos;
using BaitapcuoikyBE.Models;

namespace BaitapcuoikyBE.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _db;
    public ProductsController(AppDbContext db) => _db = db;

    [HttpGet]
    public IActionResult GetAll() => Ok(_db.Products.ToList());

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var p = _db.Products.Find(id);
        if (p == null) return NotFound();
        return Ok(p);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult Create([FromBody] ProductCreateDto dto)
    {
        var p = new Product 
        { 
            Name = dto.Name, 
            Price = dto.Price, 
            Description = dto.Description, 
            Stock = dto.Stock 
        };
        _db.Products.Add(p);
        _db.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = p.Id }, p);
    }

    // [CHỨC NĂNG MỚI] Cập nhật sản phẩm
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] ProductUpdateDto dto)
    {
        var p = _db.Products.Find(id);
        if (p == null) return NotFound("Không tìm thấy sản phẩm");

        p.Name = dto.Name;
        p.Price = dto.Price;
        p.Description = dto.Description;
        p.Stock = dto.Stock;

        _db.SaveChanges();
        return Ok(p);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var p = _db.Products.Find(id);
        if (p == null) return NotFound();
        _db.Products.Remove(p);
        _db.SaveChanges();
        return NoContent();
    }
}