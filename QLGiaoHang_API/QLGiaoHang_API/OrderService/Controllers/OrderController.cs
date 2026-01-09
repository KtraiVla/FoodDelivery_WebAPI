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

        [HttpGet("my-order")] // lấy danh sách đơn hàng
        public IActionResult GetMyOrrder([FromQuery] string? status)
        {
            var userId = int.Parse(User.FindFirst("MaTK")?.Value ?? "0");
            var result = _orderService.GetMyOrders(userId, status);
            return Ok(ApiResponse.Ok(result));
        }

        // PUT: api/order/{id}/cancel
        [HttpPut("{id}/cancel")]
        public IActionResult CancelOrder(int id)
        {
            var userId = int.Parse(User.FindFirst("MaTK")?.Value ?? "0");
            var result = _orderService.CancelOrder(userId, id);
            return result.Success ? Ok(ApiResponse.Ok(null, result.Message)) : BadRequest(ApiResponse.Fail(result.Message));
        }
    }
}
