using System.ComponentModel.DataAnnotations;

namespace BaitapcuoikyBE.Dtos;

public record ProductCreateDto(
    [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")] 
    string Name, 

    [Range(0, double.MaxValue, ErrorMessage = "Giá không được âm")] 
    decimal Price, 

    string? Description, 

    [Range(0, int.MaxValue, ErrorMessage = "Tồn kho không được âm")] 
    int Stock
);

public record ProductUpdateDto(
    [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")] 
    string Name, 

    [Range(0, double.MaxValue, ErrorMessage = "Giá không được âm")] 
    decimal Price, 

    string? Description, 

    [Range(0, int.MaxValue, ErrorMessage = "Tồn kho không được âm")] 
    int Stock
);