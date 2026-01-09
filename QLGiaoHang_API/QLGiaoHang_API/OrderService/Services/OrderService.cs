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

        // Lấy lịch sử đơn hàng của người dùng (khách hàng)
        public List<OrderResponseDto> GetMyOrders(int userId, string? statusFilter)
        {
            var paramsList = new SqlParameter[] {
                new SqlParameter("@MaTK", userId),
                new SqlParameter("@TrangThaiFilter", (object)statusFilter ?? DBNull.Value)
            };
            return MapToOrderList("sp_LayLichSuDonHang", paramsList);
        }

        // hủy đơn hàng
        public (bool Success, string Message) CancelOrder(int userId, int orderId)
        {
            var paramsList = new SqlParameter[] {
                new SqlParameter("@MaTK", userId),
                new SqlParameter("@MaDonHang", orderId)
            };
            return ExecuteAction("sp_KhachHuyDon", paramsList);
        }

        // ================= NHÀ HÀNG & ADMIN =================
        public List<OrderResponseDto> GetMerchantOrders(int userId)
        {
            return MapToOrderList("sp_LayDonHangNhaHang", new SqlParameter[] { new SqlParameter("@MaTK", userId) });
        }

        public List<OrderResponseDto> GetAllOrders()
        {
            return MapToOrderList("sp_LayTatCaDonHang", null);
        }

        public (bool Success, string Message) UpdateOrderStatus(int orderId, string newStatus)
        {
            var paramsList = new SqlParameter[] {
                new SqlParameter("@MaDonHang", orderId),
                new SqlParameter("@TrangThaiMoi", newStatus)
            };
            return ExecuteAction("sp_NhaHangCapNhatTrangThai", paramsList);
        }

        public OrderStatsDto GetStats()
        {
            DataTable dt = _helper.ExecuteQuery("sp_ThongKeDonHang");
            if (dt.Rows.Count > 0)
            {
                return new OrderStatsDto
                {
                    TongDon = Convert.ToInt32(dt.Rows[0]["TongDon"]),
                    DoanhThu = dt.Rows[0]["DoanhThu"] != DBNull.Value ? Convert.ToDecimal(dt.Rows[0]["DoanhThu"]) : 0,
                    DonHuy = Convert.ToInt32(dt.Rows[0]["DonHuy"])
                };
            }
            return new OrderStatsDto();
        }

        // ================= SHIPPER =================
        public List<OrderResponseDto> GetPendingOrdersForShipper()
        {
            return MapToOrderList("sp_LayDonHangChoShipper", null);
        }

        public (bool Success, string Message) ShipperReceiveOrder(int userId, int orderId)
        {
            var paramsList = new SqlParameter[] {
                new SqlParameter("@MaTK", userId),
                new SqlParameter("@MaDonHang", orderId)
            };
            return ExecuteAction("sp_ShipperNhanDon", paramsList);
        }

        public List<OrderResponseDto> GetShipperTasks(int userId)
        {
            return MapToOrderList("sp_LayDonCuaShipper", new SqlParameter[] { new SqlParameter("@MaTK", userId) });
        }

        public (bool Success, string Message) ShipperCompleteOrder(int userId, int orderId)
        {
            var paramsList = new SqlParameter[] {
                new SqlParameter("@MaTK", userId),
                new SqlParameter("@MaDonHang", orderId)
            };
            return ExecuteAction("sp_ShipperHoanThanh", paramsList);
        }

        private List<OrderResponseDto> MapToOrderList(string spName, SqlParameter[]? parameters)
        {
            var list = new List<OrderResponseDto>();
            DataTable dt = _helper.ExecuteQuery(spName, parameters);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new OrderResponseDto
                {
                    MaDonHang = Convert.ToInt32(row["MaDonHang"]),
                    NgayDat = Convert.ToDateTime(row["NgayDat"]),
                    TrangThai = row["TrangThai"].ToString(),
                    TongTien = Convert.ToDecimal(row["TongTien"]),
                    DiaChiGiao = row["DiaChiGiao"].ToString(),
                    PhuongThucThanhToan = row["PhuongThucThanhToan"].ToString(),
                    TenNhaHang = row.Table.Columns.Contains("TenNhaHang") ? row["TenNhaHang"].ToString() : "",
                    TenKhachHang = row.Table.Columns.Contains("TenKhachHang") ? row["TenKhachHang"].ToString() : "",
                    // Các cột khác kiểm tra tương tự...
                });
            }
            return list;
        }

        private (bool Success, string Message) ExecuteAction(string spName, SqlParameter[] parameters)
        {
            try
            {
                DataTable dt = _helper.ExecuteQuery(spName, parameters);
                if (dt.Rows.Count > 0)
                {
                    int code = Convert.ToInt32(dt.Rows[0]["ResultCode"]);
                    string msg = dt.Rows[0]["Message"].ToString();
                    return (code == 1, msg);
                }
                return (false, "Không có phản hồi từ database");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
