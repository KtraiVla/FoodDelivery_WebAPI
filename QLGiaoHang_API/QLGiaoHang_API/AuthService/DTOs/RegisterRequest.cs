using Shared.Enums;
using System.ComponentModel.DataAnnotations;
namespace AuthService.DTOs
{
    public class RegisterRequest :IValidatableObject
    {
        [Required(ErrorMessage ="Tên đăng nhập không được để trống")]
        public string Username { get; set; }
        [Required(ErrorMessage ="Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage ="Mật khẩu phải từ 6 ký tự trở lên")]
        public string Password { get; set; }
        public UserRole VaiTro { get; set; }

        // thông tin chung 
        [Required(ErrorMessage ="Họ tên không được để trống")]
        public string HoTen { get; set; }
        [Required(ErrorMessage ="Số điện thoại là bắt buộc")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage ="Số điện thoại phải bắt đầu bằng số 0 và có đúng 10 chữ số")]
        public string SoDienThoai { get; set; }
        // thông tin riêng 
        public string? DiaChi { get; set; }     // cho khachhang
        public string? BienSoXe { get; set; }   // cho shipper
        public string? MaCode { get; set; }     // cho nhanvien

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // nếu là nhân viên thì phải nhập mã code
            if(VaiTro == UserRole.NhanVien)
            {
                if (string.IsNullOrEmpty(MaCode))
                {
                    yield return new ValidationResult("Nhân viên phải nhập mã code nhà hàng",
                    new[] { nameof(MaCode) });
                }
            }

            // shipper thì phải nhập biển số xe
            if(VaiTro == UserRole.Shipper)
            {
                if (string.IsNullOrEmpty(BienSoXe))
                {
                    yield return new ValidationResult("Shipper phải nhập biển số xe",
                    new[] { nameof(BienSoXe) });
                }
            }
        }

    }
}
