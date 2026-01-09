namespace OrderService.DTOs
{
        // 1. Chi tiết từng món ăn trong giỏ
        public class CartItemDto
        {
            public int MaMonAn { get; set; }
            public int SoLuong { get; set; }    

        }
        public class CreateOrderRequest
        {
            public int MaNhaHang { get; set; }
            public string DiaChiGiao { get; set; }
            public string? GhiChu { get; set; }
            public string PhuongThucThanhToan { get; set; } = "TienMat"; // mặc định là tiền mặt
            public List<CartItemDto>  GioHang { get; set; } 
        }
    
    // DTO để hiện thị dánh sách đơn hàng 
    public class OrderResponseDto
    {
        public int MaDonHang { get; set; }
        public DateTime NgayDat { get; set; }
        public string TrangThai { get; set; }
        public decimal TongTien { get; set; }
        public string TenNhaHang { get; set; } // Dành cho khách xem
        public string DiaChiGiao { get; set; }
        public string PhuongThucThanhToan { get; set; }

        // Thông tin cho Shipper/Nhà hàng xem
        public string? TenKhachHang { get; set; }
        public string? SoDienThoai { get; set; }
        public string? DiaChiLay { get; set; }
    }


    // DTO cho thống kê
    public class OrderStatsDto
    {
        public int TongDon { get; set; }
        public decimal DoanhThu { get; set; }
        public int DonHuy { get; set; }
    }

}
