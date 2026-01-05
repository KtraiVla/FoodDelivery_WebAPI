using Microsoft.Data.SqlClient;
using OrderService.DTOs;
using OrderService.Interfaces;
using Shared.Helpers;
using System.Data;
using System.Text.Json;   // xử lý json

namespace OrderService.Services
{
    public class OrderService : IOrderService
    {
        private readonly SqlHelper _helper; 
        public OrderService(SqlHelper helper)
        {
            _helper = helper;
        }

        public (int ResultCode, string Message, int OrderId) CreateOrder(int userId, CreateOrderRequest request)
        {
            //1. kiểm tra giỏ hàng 
            if (request.GioHang == null || request.GioHang.Count == 0)
            {
                return (-1, "Giỏ hàng không được để trống", 0);
            }

            // chuyển list món ăn --> chuỗi Json
            string jsonCart = JsonSerializer.Serialize(request.GioHang, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaTK" , userId),
                new SqlParameter("@MaNhaHang", request.MaNhaHang),
                new SqlParameter("@DiaChiGiao", request.DiaChiGiao),
                new SqlParameter("@GhiChu", (object)request.GhiChu ?? DBNull.Value),
                new SqlParameter("@PhuongThucThanhToan", request.PhuongThucThanhToan),
                new SqlParameter("@ListMonAnJson", jsonCart) 
            };

            try
            {
                DataTable dt = _helper.ExecuteQuery("sp_TaoDonHang", parameters);
                if(dt.Rows.Count > 0)
                {
                    int code = Convert.ToInt32(dt.Rows[0]["ResultCode"]);
                    string msg = dt.Rows[0]["Message"].ToString();
                    int orderId = 0; 
                    
                    if(code == 1 && dt.Columns.Contains("OrderID") && dt.Rows[0]["OrderID"] != DBNull.Value)
                    {
                        orderId = Convert.ToInt32(dt.Rows[0]["OrderID"]);
                    }

                    return (code, msg, orderId);

                }
            }
            catch (Exception ex)
            {
                return(0, "Lỗi Server: " + ex.Message, 0);
            }

            return (0, "Lỗi kết nối cơ sở dữ liệu", 0);
        }
    }
}
