using System.ComponentModel.DataAnnotations;

namespace BaitapcuoikyBE.Dtos;

public record OrderItemDto(
    [Required] int ProductId, 
    
    [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")] 
    int Quantity
);

public record OrderCreateDto(
    int CustomerId, // Sẽ được kiểm tra logic ở Controller
    
    [Required(ErrorMessage = "Giỏ hàng không được để trống")] 
    List<OrderItemDto> Items
);