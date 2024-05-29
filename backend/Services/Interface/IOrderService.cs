using backend.Models;

namespace backend.Services.Interface
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrdersAsync(string userId);
        Task<Order> GetOrderByIdAsync(int orderId, string userId);
        Task<Order> AddOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(int orderId);
    }
}
