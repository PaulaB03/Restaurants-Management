using backend.Data;
using backend.Models;
using backend.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;

        public OrderService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(string userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int orderId, string userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            order.TotalPrice = await CalculateTotalPriceAsync(order.OrderItems);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return false;
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<float> CalculateTotalPriceAsync(List<OrderItem> orderItems)
        {
            float totalPrice = 0;
            foreach (var item in orderItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null)
                {
                    totalPrice += product.Price * item.Quantity;
                }
            }
            return totalPrice;
        }
    }
}
