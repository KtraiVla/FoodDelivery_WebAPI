using AuthService.DTOs;
using AuthService.Interfaces;
using Shared.Helpers;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AuthService.Services
{
    public class AuthService : IAuthService
    {
        private readonly SqlHelper _helper;
        public AuthService(SqlHelper helper)
        {
            _helper = helper;
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


    }
}
