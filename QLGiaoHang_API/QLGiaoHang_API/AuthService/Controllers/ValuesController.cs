using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    // [1] Định tuyến: Đường dẫn sẽ là /api/values
    [Route("api/[controller]")]
    // [2] Đánh dấu đây là API để Swagger nhận diện
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // [3] Phương thức GET: Lấy dữ liệu
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new string[] { "Giá trị 1", "Giá trị 2" });
        }

        // [4] Phương thức GET theo ID: /api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok($"Bạn đang tìm giá trị số {id}");
        }
    }
}