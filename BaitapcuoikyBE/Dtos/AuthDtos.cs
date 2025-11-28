using System.ComponentModel.DataAnnotations;

namespace BaitapcuoikyBE.Dtos;

public record RegisterDto(
    [Required(ErrorMessage = "Email là bắt buộc")] 
    [EmailAddress(ErrorMessage = "Email không hợp lệ")] 
    string Email, 

    [Required(ErrorMessage = "Mật khẩu là bắt buộc")] 
    [MinLength(6, ErrorMessage = "Mật khẩu phải từ 6 ký tự")] 
    string Password, 

    [Required] 
    string Role // "Admin" hoặc "User"
);

public record LoginDto(
    [Required(ErrorMessage = "Email là bắt buộc")] 
    [EmailAddress] 
    string Email, 

    [Required(ErrorMessage = "Mật khẩu là bắt buộc")] 
    string Password
);