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
    }
}
