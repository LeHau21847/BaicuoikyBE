namespace BaitapcuoikyBE.Models;
public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Pending";
    public decimal TotalPrice { get; set; }
    public Customer? Customer { get; set; }
    public List<OrderDetail> OrderDetails { get; set; } = new();
}