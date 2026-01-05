using AuthService.DTOs;

namespace AuthService.Interfaces
{
    public interface IAuthService
    {
        // Đăng ký: Trả về (Mã kết quả, Thông báo)
        (int ResultCode, string Message) Register(RegisterRequest request);

        // Đăng nhập: Trả về UserDto (hoặc null nếu sai)
        UserDto? Login(LoginRequest request);

    }
}
