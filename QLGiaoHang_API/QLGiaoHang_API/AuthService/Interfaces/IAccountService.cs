using AuthService.DTOs;

namespace AuthService.Interfaces
{
    public interface IAccountService
    {
        // lấy thông tin cá nhân 
        UserDto? GetProfile(int userId);
        
        (bool Success, string Message) UpdateProfile(int userId, UpdateProfileRepuest repuest); 
    }
}
