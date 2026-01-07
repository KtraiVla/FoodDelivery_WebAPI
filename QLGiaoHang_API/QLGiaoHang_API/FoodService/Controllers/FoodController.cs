using FoodService.DTOs;
using FoodService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IFoodService _foodService;

        public FoodController(IFoodService foodService)
        {
            _foodService = foodService;
        }
        // ------------ NHÀ HÀNG -------------

        
        /// Lấy danh sách tất cả nhà hàng
        
        [HttpGet("nhahang")]
        public async Task<IActionResult> GetAllNhaHang()
        {
            var result = await _foodService.GetAllNhaHang();
            return Ok(result);
        }

        
        /// Lấy thông tin nhà hàng theo ID
        
        [HttpGet("nhahang/{maNhaHang}")]
        public async Task<IActionResult> GetNhaHangById(int maNhaHang)
        {
            var result = await _foodService.GetNhaHangById(maNhaHang);

            if (result == null)
                return NotFound("Không tìm thấy nhà hàng");

            return Ok(result);
        }

        // Thêm nhà hàng
        [HttpPost("Create-nhahang")]
        public async Task<IActionResult> CreateNhaHang([FromBody] NhaHang nhaHang)
        {
            if (nhaHang == null)
                return BadRequest("Dữ liệu không hợp lệ");

            var result = await _foodService.CreateNhaHang(nhaHang);

            if (!result)
                return StatusCode(500, "Thêm nhà hàng thất bại");

            return Ok("Thêm nhà hàng thành công");
        }

        // Sửa nhà hàng
        [HttpPut("Update-NhaHang")]
        public async Task<IActionResult> UpdateNhaHang([FromBody] NhaHang nhaHang)
        {
            if (nhaHang == null || nhaHang.MaNhaHang <= 0)
                return BadRequest("Dữ liệu không hợp lệ");

            var result = await _foodService.Update(nhaHang);

            if (!result)
                return StatusCode(500, "Sửa nhà hàng thất bại");

            return Ok("Sửa thông tin nhà hàng thành công");
        }

        // ----------------- DANH MỤC -----------------


        /// Lấy tất cả danh mục

        [HttpGet("danhmuc")]
        public async Task<IActionResult> GetAllDanhMuc()
        {
            var result = await _foodService.GetAllDanhMuc();
            return Ok(result);
        }

        
        /// Lấy danh mục theo nhà hàng
        
        [HttpGet("nhahang/{maNhaHang}/danhmuc")]
        public async Task<IActionResult> GetDanhMucByNhaHang(int maNhaHang)
        {
            var result = await _foodService.GetDanhMucByNhaHang(maNhaHang);
            return Ok(result);
        }

        // ---------------------- MÓN ĂN ----------------------


        /// Lấy món ăn theo danh mục

        [HttpGet("nhahang/{maNhaHang}/danhmuc/{maDanhMuc}/monan")]
        public async Task<IActionResult> GetMonAnByDanhMuc(int maDanhMuc, int maNhaHang)
        {
            var result = await _foodService.GetMonAnByDanhMuc(maDanhMuc, maNhaHang);
            return Ok(result);
        }


        /// Lấy món ăn theo nhà hàng

        [HttpGet("nhahang/{maNhaHang}/monan")]
        public async Task<IActionResult> GetMonAnByNhaHang(int maNhaHang)
        {
            var result = await _foodService.GetMonAnByNhaHang(maNhaHang);
            return Ok(result);
        }

       
        /// Lấy chi tiết món ăn theo ID
        
        [HttpGet("monan/{maMonAn}")]
        public async Task<IActionResult> GetMonAnById(int maMonAn)
        {
            var result = await _foodService.GetMonAnById(maMonAn);

            if (result == null)
                return NotFound("Không tìm thấy món ăn");

            return Ok(result);
        }

        // Thêm món ăn
        [HttpPost("Create-MonAn")]
        public async Task<IActionResult> Create([FromBody] MonAn monAn)
        {
            if (monAn == null)
                return BadRequest("Dữ liệu không hợp lệ");

            var result = await _foodService.Create(monAn);

            if (!result)
                return StatusCode(500, "Thêm món ăn thất bại");

            return Ok("Thêm món ăn thành công");
        }
        // sửa món ăn
        [HttpPut("Update-MonAn")]
        public async Task<IActionResult> Update([FromBody] MonAn monAn)
        {
            if (monAn == null || monAn.MaMonAn <= 0)
                return BadRequest("Dữ liệu không hợp lệ");

            var result = await _foodService.Update(monAn);

            if (!result)
                return StatusCode(500, "Sửa món ăn thất bại");

            return Ok("Sửa món ăn thành công");
        }

        // Xóa món ăn
        [HttpDelete("Delete-MonAn/{maMonAn}")]
        public async Task<IActionResult> Delete(int maMonAn)
        {
            if (maMonAn <= 0)
                return BadRequest("Mã món ăn không hợp lệ");

            var result = await _foodService.Delete(maMonAn);

            if (!result)
                return StatusCode(500, "Xóa món ăn thất bại");

            return Ok("Xóa món ăn thành công");
        }
    }
}
