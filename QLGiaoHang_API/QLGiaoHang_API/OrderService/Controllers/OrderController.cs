using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.DTOs; 
using OrderService.Interfaces;
using Shared.DTOs;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("checkout")]
        public IActionResult CreateOrder([FromBody] CreateOrderRequest request)
        {
            // lấy userid từ token (để biết ai đăng đặt)
            var userIdClaim = User.FindFirst("MaTK")?.Value;

            if(string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized(ApiResponse.Fail("Bạn cần đăng nhập để thực hiện chức năng này"));
            }

            if(!int.TryParse(userIdClaim, out var userId))
            {
               return BadRequest(ApiResponse.Fail("Token không hợp lệ"));
            }

            var result = _orderService.CreateOrder(userId, request);

            if(result.ResultCode == 1)
            {
                return Ok(ApiResponse.Ok(result.OrderId, result.Message));
            }
            else
            {
                return BadRequest(ApiResponse.Fail(result.Message));
            }
            
        }

    }
}
