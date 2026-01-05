using AuthService.Interfaces;
using Shared.DTOs; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            // lấy userid từ token
            var userIdClaim = User.FindFirst("MaTK")?.Value;
            if(string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized(ApiResponse.Fail("Token không hợp lệ hoặc đã hết hạn"));
            }

            if(!int.TryParse(userIdClaim, out var userId))
            {
                return BadRequest(ApiResponse.Fail("Mã tài khoản không đúng"));
            };

            var userProfile = _accountService.GetProfile(userId);
            if(userProfile != null)
            {
                return Ok(ApiResponse.Ok(userProfile, "Lấy thông tin thành công"));
            }

            return NotFound(ApiResponse.Fail("Không tìm thấy thông tin hồ sơ"));
        }
    }
}
