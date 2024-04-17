using backend.Models;
using backend.Services;
using backend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Roles = "User")]
        [HttpPost("AddOrder")]
        public async Task<IActionResult> AddOrder(Order order)
        {
            var result = await _orderService.AddOrder(order);
            if (result)
            {
                return Ok(order);
            }
            return BadRequest("Failed to add order!");
        }

        [Authorize(Roles = "User")]
        [HttpPost("AddOrderItem")]
        public async Task<IActionResult> AddOrderItem(OrderItem orderItem)
        {
            var result = await _orderService.AddOrderItem(orderItem);
            if (result)
            {
                return Ok(orderItem);
            }
            return BadRequest("Failed to add order item!");
        }

        [Authorize(Roles = "User")]
        [HttpDelete("{orderItemId}")]
        public async Task<IActionResult> DeleteOrderItem(int orderItemId)
        {
            var result = await _orderService.DeleteOrderItem(orderItemId);
            if (result)
            {
                return Ok("Order item deleted");
            }
            return BadRequest("Failed to delete order item!");
        }

        [Authorize(Roles = "User")]
        [HttpPut("IncreaseOrderItemQuantity{orderItemId}")]
        public async Task<IActionResult> IncreaseOrderItemQuantity(int orderItemId)
        {
            var result = await _orderService.IncreaseOrderItemQuantity(orderItemId);
            if (result)
            {
                return Ok("Quantity increased");
            }
            return BadRequest("Failed to increase quantity");
        }

        [Authorize(Roles = "User")]
        [HttpPut("DecreaseOrderItemQuantity{orderItemId}")]
        public async Task<IActionResult> DecreaseOrderItemQuantity(int orderItemId)
        {
            var result = await _orderService.IncreaseOrderItemQuantity(orderItemId);
            if (result)
            {
                return Ok("Quantity decreased");
            }
            return BadRequest("Failed to decrease quantity");
        }
    }
}
