using Shared.Enums;
namespace AuthService.DTOs
{
    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole VaiTro { get; set; }

        // thông tin chung 
        public string HoTen { get; set; }
        public string SoDienThoai { get; set; }
        // thông tin riêng 
        public string? DiaChi { get; set; }     // cho khachhang
        public string? BienSoXe { get; set; }   // cho shipper
        public int? MaCode { get; set; }     // cho nhanvien
    }
}
