using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BaitapcuoikyBE.Data;
using BaitapcuoikyBE.Dtos;
using BaitapcuoikyBE.Models;

namespace BaitapcuoikyBE.Controllers;

[ApiController]
[Route("api/products")] // Chuẩn RESTful
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _db;
    public ProductsController(AppDbContext db) => _db = db;

    [HttpGet]
    public IActionResult GetAll() => Ok(_db.Products.ToList());

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult Create([FromBody] ProductCreateDto dto)
    {
        var p = new Product { Name = dto.Name, Price = dto.Price, Description = dto.Description, Stock = dto.Stock };
        _db.Products.Add(p);
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