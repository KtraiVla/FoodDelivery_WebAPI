using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string VaiTro { get; set; }
      
         // thông tin chung 
        public string HoTen { get; set; }
        public string SoDienThoai { get; set; }
        // thông tin riêng 
        public string? DiaChi { get; set; }     // cho khachhang
        public string? BienSoXe { get; set; }   // cho shipper
        public int? MaNhaHang { get; set; }     // cho nhanvien
          
    }
}
