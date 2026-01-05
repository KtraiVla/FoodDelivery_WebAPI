using AuthService.DTOs;
using AuthService.Interfaces;
using Shared.Helpers;
using Microsoft.Data.SqlClient;
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
    }
}
