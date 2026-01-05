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

}
