using backend.Models;
using System.Reflection;

namespace backend.Services.Interface
{
    public interface IRestaurantService
    {
        Task<bool> AddRestaurant(Restaurant restaurant);
        Task<bool> DeleteRestaurant(int restaurantId);
        Task<Restaurant> GetRestaurantById(int restaurantId);
        Task<List<Restaurant>> GetRestaurants();
    }
}
