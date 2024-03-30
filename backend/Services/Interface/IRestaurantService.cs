using backend.Models;

namespace backend.Services.Interface
{
    public interface IRestaurantService
    {
        Task<bool> AddRestaurant(Restaurant restaurant);
        Task<bool> DeleteRestaurant(int restaurantId);
    }
}
