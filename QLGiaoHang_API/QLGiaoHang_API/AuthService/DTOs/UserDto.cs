namespace AuthService.DTOs
{
    public class UserDto
    {
        // thông tin chung
        public int MaTK { get; set; }
        public string Username { get; set; }
        public string VaiTro { get; set; }

        // thông tin hiển thị
        public string HoTen { get; set; }

        // thông tin riêng
        public int? MaNhaHang { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string BienSoXe { get; set; }
    }
}
