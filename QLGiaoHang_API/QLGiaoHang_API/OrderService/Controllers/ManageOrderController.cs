using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Interfaces;
using Shared.DTOs;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ManageOrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public ManageOrderController(IOrderService orderService)
        {
            
            _orderService = orderService;
        }

        [HttpGet("merchant/list")]  // lấy danh sách đơn của quán
        public IActionResult GetMerchantOrders()
        {
            var userId = int.Parse(User.FindFirst("MaTK")?.Value ?? "0");
            var result = _orderService.GetMerchantOrders(userId);
            return Ok(ApiResponse.Ok(result));
        }

        // GET: api/order/admin/all
        [HttpGet("admin/all")]   // lấy toàn bộ đơn hàng dành cho admin 
        public IActionResult GetAllOrders()
        {
            var result = _orderService.GetAllOrders();
            return Ok(ApiResponse.Ok(result));
        }

        // PUT: api/order/{id}/confirm
        [HttpPut("{id}/confirm")]
        public IActionResult ConfirmOrder(int id)
        {
            var result = _orderService.UpdateOrderStatus(id, "DangChuanBi");
            return result.Success ? Ok(ApiResponse.Ok(null, result.Message)) : BadRequest(ApiResponse.Fail(result.Message));
        }

        // PUT: api/order/{id}/ready
        [HttpPut("{id}/ready")]
        public IActionResult OrderReady(int id)
        {
            var result = _orderService.UpdateOrderStatus(id, "SanSangGiao");
            return result.Success ? Ok(ApiResponse.Ok(null, result.Message)) : BadRequest(ApiResponse.Fail(result.Message));
        }

        // GET: api/order/stats
        [HttpGet("stats")]
        public IActionResult GetStats()
        {
            var result = _orderService.GetStats();
            return Ok(ApiResponse.Ok(result));
        }
    }
}
