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
    public class ShipperController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public ShipperController(IOrderService orderService) { _orderService = orderService; }

        // GET: api/order/shipping/pending
        [HttpGet("pending")]
        public IActionResult GetPendingOrders()
        {
            var result = _orderService.GetPendingOrdersForShipper();
            return Ok(ApiResponse.Ok(result));
        }

        // PUT: api/order/shipping/{id}/receive
        [HttpPut("{id}/receive")]
        public IActionResult ReceiveOrder(int id)
        {
            var userId = int.Parse(User.FindFirst("MaTK")?.Value ?? "0");
            var result = _orderService.ShipperReceiveOrder(userId, id);
            return result.Success ? Ok(ApiResponse.Ok(null, result.Message)) : BadRequest(ApiResponse.Fail(result.Message));
        }

        // GET: api/order/shipping/my-tasks
        [HttpGet("my-tasks")]
        public IActionResult GetMyTasks()
        {
            var userId = int.Parse(User.FindFirst("MaTK")?.Value ?? "0");
            var result = _orderService.GetShipperTasks(userId);
            return Ok(ApiResponse.Ok(result));
        }

        // PUT: api/order/shipping/{id}/complete
        [HttpPut("{id}/complete")]
        public IActionResult CompleteOrder(int id)
        {
            var userId = int.Parse(User.FindFirst("MaTK")?.Value ?? "0");
            var result = _orderService.ShipperCompleteOrder(userId, id);
            return result.Success ? Ok(ApiResponse.Ok(null, result.Message)) : BadRequest(ApiResponse.Fail(result.Message));
        }
    }
}
