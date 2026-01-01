using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    // 1. DTO chính: Chứa thông tin tổng quan của đơn hàng
    public class CreateOrderRequest
    {
        public int MaKhachHang { get; set; }
        public int MaNhaHang { get; set; }

        public string DiaChiGiao { get; set; }
        public string GhiChu { get; set; }

        // "TienMat" hoặc "ChuyenKhoan"
        public string PhuongThucThanhToan { get; set; }

        public decimal TongTien { get; set; } // Tổng tiền (Frontend tính hoặc Backend tính lại)

        // Quan trọng: Danh sách các món ăn khách chọn
        public List<CartItemDto> Items { get; set; }
    }

    // 2. DTO con: Đại diện cho từng món ăn trong giỏ
    public class CartItemDto
    {
        public int MaMonAn { get; set; }
        public string TenMon { get; set; } // Gửi kèm tên để lỡ giá thay đổi còn biết
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
    }

}
