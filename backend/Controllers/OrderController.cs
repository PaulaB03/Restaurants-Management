using backend.Models;
using backend.Services;
using backend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize]
        [Route("GetOrders/{userId}")]
        public async Task<IActionResult> GetOrders(string userId)
        {
            var orders = await _orderService.GetOrdersAsync(userId);
            return Ok(orders);
        }

        [HttpGet]
        [Authorize]
        [Route("GetOrderById/{id}/{userId}")]
        public async Task<IActionResult> GetOrderById(int id, string userId)
        {
            var order = await _orderService.GetOrderByIdAsync(id, userId);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpPost]
        [Authorize]
        [Route("AddOrder")]
        public async Task<IActionResult> AddOrder(Order order)
        {
            order.OrderDate = DateTime.UtcNow;
            var createdOrder = await _orderService.AddOrderAsync(order);
            return Ok(createdOrder);
        }

        [HttpDelete]
        [Authorize]
        [Route("DeleteOrder/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
    }

}