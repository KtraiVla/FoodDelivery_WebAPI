using AuthService.DTOs;
using AuthService.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Shared.Helpers;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Services
{
    public class AuthService : IAuthService
    {
        private readonly SqlHelper _helper;
        private readonly IConfiguration _configuration; // đọc key trong app setting

        public AuthService(SqlHelper helper, IConfiguration configuration)
        {
            _helper = helper;
            _configuration = configuration;
        }

        //  Đăng ký
        public (int ResultCode, string Message) Register(RegisterRequest request)
        {
            // chuẩn bị tham số gửi xuống SQL
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Username", request.Username),
                new SqlParameter("@MatKhau", request.Password),
                new SqlParameter("@VaiTro", request.VaiTro.ToString()),
                new SqlParameter("@HoTen", request.HoTen),
                new SqlParameter("@SoDienThoai", request.SoDienThoai),
                new SqlParameter("@DiaChi", (object)request.DiaChi ?? DBNull.Value),
                new SqlParameter("@BienSoXe", (object)request.BienSoXe ?? DBNull.Value),
                new SqlParameter("@MaCode", (object)request.MaCode ?? DBNull.Value)
            };

            try
            {
                DataTable dt = _helper.ExecuteQuery("sp_DangKy", parameters);
                if(dt.Rows.Count > 0)
                {
                    int code = Convert.ToInt32(dt.Rows[0][0]);
                    string msg = "";
                    switch(code)
                    {
                        case 1: msg = "Đăng ký thành công"; break;
                        case -1: msg = "Tài khoản đã tồn tại"; break;
                        case -2: msg = "Mã code nhà hàng không đúng"; break;
                        case 0: msg = "Lỗi hệ thống SQL"; break;
                        default: msg = "Lỗi không xác định"; break;
                    }

                    return (code, msg);
                }
            }
            catch (Exception ex)
            {
                return (0, "Lỗi Server: " + ex.Message);
            }

            return (0, "Lỗi kết nối Database");
        }

        public UserDto? Login(LoginRequest request)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Username", request.Username),
                new SqlParameter("@MatKhau", request.MatKhau)
            };

            try
            {
                DataTable dt = _helper.ExecuteQuery("sp_DangNhap", parameters);
                if(dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    return new UserDto
                    {
                        MaTK = Convert.ToInt32(row["MaTK"]),
                        Username = row["Username"].ToString(),
                        VaiTro = row["VaiTro"].ToString(),
                        HoTen = row.Table.Columns.Contains("HoTen") ? row["HoTen"].ToString() : "Người dùng",
                        SoDienThoai = row.Table.Columns.Contains("SoDienThoai") && row["SoDienThoai"] != DBNull.Value ? row["SoDienThoai"].ToString() : null,
                        MaNhaHang = row.Table.Columns.Contains("MaNhaHang") && row["MaNhaHang"] != DBNull.Value
                            ? Convert.ToInt32(row["MaNhaHang"]) : null,
                        DiaChi = row.Table.Columns.Contains("DiaChi") && row["DiaChi"] != DBNull.Value
                         ? row["DiaChi"].ToString() : null,
                        BienSoXe = row.Table.Columns.Contains("BienSoXe") && row["BienSoXe"] != DBNull.Value
                           ? row["BienSoXe"].ToString() : null

                    };

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi đăng nhập: " + ex.Message);    
            }

            return null;
        }

        // Hàm tạo token từ UserDto
        public string CreateToken(UserDto user)
        {
            var claims = new List<Claim>
            {
                new Claim("MaTK", user.MaTK.ToString()),
                new Claim(ClaimTypes.Role, user.VaiTro),
                new Claim("Username", user.Username)
            };

            // nếu là nhân viên nhà hàng, nhét thêm mã nhà hàng vào token 
            if (user.MaNhaHang.HasValue)
            {
                claims.Add(new Claim("MaNhaHang", user.MaNhaHang.Value.ToString()));
            }

            // lấy secret key từ file cấu hình 
            var keyString = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(keyString))
                throw new Exception("Chưa cấu hình Jwt:Key trong appsetting.json");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // cấu hình token 
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7), // Token hết hạn sau 7 ngày
                SigningCredentials = creds,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            // 4. Tạo chuỗi Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


    }
}
