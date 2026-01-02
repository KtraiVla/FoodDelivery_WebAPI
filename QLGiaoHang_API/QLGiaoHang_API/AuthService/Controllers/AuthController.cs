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

            if( user != null )
            {
                // thành công
                return Ok(ApiResponse.Ok(user, "Đăng nhập thành công"));
            }

            return Unauthorized(ApiResponse.Fail("Tài khoản hoặc mật khẩu không chính xác"));
        }
    }
}
