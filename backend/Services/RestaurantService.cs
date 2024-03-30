using backend.Data;
using backend.Models;
using backend.Services.Interface;

namespace backend.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly DataContext _context;

        public RestaurantService(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> AddRestaurant(Restaurant restaurant)
        {
            _context.Restaurants.Add(restaurant);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteRestaurant(int restaurantId)
        {
            var restaurant = await _context.Restaurants.FindAsync(restaurantId);
            if (restaurant == null)
            {
                return false;
            }

            _context.Restaurants.Remove(restaurant);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
