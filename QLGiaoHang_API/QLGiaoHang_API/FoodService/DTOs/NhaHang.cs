using System.Data;

namespace FoodService.DTOs
{
    public class NhaHang
    {
        public int MaNhaHang { get; set; }
        public string TenNhaHang { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string HinhAnh { get; set; }
        public decimal MinOrder { get; set; }
        public string MaCode { get; set; }
    }
}
