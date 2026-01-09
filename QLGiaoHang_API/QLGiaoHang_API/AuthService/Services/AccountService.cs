using AuthService.DTOs;
using AuthService.Interfaces;
using Microsoft.Data.SqlClient;
using Shared.Helpers;
using System.Data; 

namespace AuthService.Services
{
    public class AccountService : IAccountService
    {
        private readonly SqlHelper _helper; 
        public AccountService(SqlHelper helper)
        {
              _helper = helper;
        }
        public UserDto? GetProfile(int userId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaTK", userId)
            };

            try 
            {
                DataTable dt = _helper.ExecuteQuery("sp_LayThongTinProfile", parameters);
                if(dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    return new UserDto
                    {
                        MaTK = Convert.ToInt32(row["MaTK"]),
                        Username = row["Username"].ToString(),
                        VaiTro = row["VaiTro"].ToString(),
                        HoTen = row["HoTen"] != DBNull.Value ? row["HoTen"].ToString() : "Chưa cập nhật",
                        SoDienThoai = row.Table.Columns.Contains("SoDienThoai") && row["SoDienThoai"] != DBNull.Value
                            ? row["SoDienThoai"].ToString() : null,
                        DiaChi = row.Table.Columns.Contains("DiaChi") && row["DiaChi"] != DBNull.Value
                            ? row["DiaChi"].ToString() : null,
                        BienSoXe = row.Table.Columns.Contains("BienSoXe") && row["BienSoXe"] != DBNull.Value
                            ? row["BienSoXe"].ToString() : null,
                        MaNhaHang = row.Table.Columns.Contains("MaNhaHang") && row["MaNhaHang"] != DBNull.Value
                            ? Convert.ToInt32(row["MaNhaHang"]) : null
                    };
                }   
            }
            catch(Exception ex)
            {
                Console.WriteLine("Lỗi AccountService: " + ex.Message);
            }

            return null; 
        }

        public  (bool Success, string Message) UpdateProfile(int userId, UpdateProfileRepuest request)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaTK", userId),
        
                 // Nếu user không nhập (null) hoặc nhập chuỗi rỗng ("") -> Gửi DBNull xuống SQL
                new SqlParameter("@HoTen", string.IsNullOrEmpty(request.HoTen) ? DBNull.Value : request.HoTen),

                new SqlParameter("@SoDienThoai", string.IsNullOrEmpty(request.SoDienThoai) ? DBNull.Value : request.SoDienThoai),

                new SqlParameter("@DiaChi", string.IsNullOrEmpty(request.DiaChi) ? DBNull.Value : request.DiaChi)
            }; 
            
            try
            {
                DataTable dt = _helper.ExecuteQuery("sp_CapNhatProfile", parameters);
                if(dt.Rows.Count > 0)
                {
                    return (true, "Cập nhật hồ sơ thành công");
                }  
            }
            catch (Exception ex)
            {
                return(false, "Lỗi server: " + ex.Message);
            }

            return (false, "Cập nhật trạng thái thất bại");
        }
    }
}
