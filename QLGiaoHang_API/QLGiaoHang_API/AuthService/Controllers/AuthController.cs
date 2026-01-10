using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AuthService.Interfaces;
using AuthService.DTOs;
using Shared.DTOs;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // API đăng ký
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } 

            var result = _authService.Register(request);
            if(result.ResultCode > 0)
            {
                return Ok(ApiResponse.Ok(result.ResultCode, result.Message));
            }

            // thất bại 
            return BadRequest(ApiResponse.Fail(result.Message));
        }

        // API đăng nhập
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _authService.Login(request);
            if (user == null) return BadRequest(ApiResponse.Fail("Sai tài khoản hoặc mật khẩu"));

            // 1. Tạo Token
            string token = _authService.CreateToken(user);

            // 2. Đóng gói (User + Token)
            var responseData = new
            {
                User = user,   // <-- Thông tin người dùng
                Token = token  // <-- Chuỗi Token quan trọng đây
            };

            // 3. TRẢ VỀ responseData (CHỨ KHÔNG PHẢI user)
            return Ok(ApiResponse.Ok(responseData, "Đăng nhập thành công"));
        }
    }
}
