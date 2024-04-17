using backend.Models;

namespace backend.Services.Interface
{
    public interface IOrderService
    {
        Task<bool> AddOrder(Order order);
        Task<bool> AddOrderItem(OrderItem orderItem);
        Task<bool> DeleteOrderItem(int orderItemId);
        Task<bool> IncreaseOrderItemQuantity(int orderItemId);
        Task<bool> DecreaseOrderItemQuantity(int orderItemId);
    }
}
