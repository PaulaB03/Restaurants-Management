using backend.Models;
using backend.Services;
using backend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddRestaurant(Restaurant restaurant)
        {
            var result = await _restaurantService.AddRestaurant(restaurant);
            if (result)
            {
                return Ok(restaurant);
            }
            return BadRequest("Failed to add restaurant!");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{restaurantId}")]
        public async Task<IActionResult> DeleteRestaurant(int restaurantId)
        {
            var result = await _restaurantService.DeleteRestaurant(restaurantId);
            if (result)
            {
                return Ok("Restaurant deleted");
            }
            return BadRequest("Failed to delete restaurant!");
        }
    }
}
