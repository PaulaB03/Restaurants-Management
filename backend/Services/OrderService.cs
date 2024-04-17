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

        public async Task<bool> AddOrder(Order order)
        {
            _context.Orders.Add(order);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddOrderItem(OrderItem orderItem)
        {
            _context.OrderItems.Add(orderItem);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteOrderItem(int orderItemId)
        {
            var orderItem = await _context.OrderItems.FindAsync(orderItemId);
            if (orderItem == null)
            {
                return false;
            }

            _context.OrderItems.Remove(orderItem);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IncreaseOrderItemQuantity(int orderItemId)
        {
            var existingOrderItem = await _context.OrderItems.FindAsync(orderItemId);
            if (existingOrderItem == null)
            {
                return false;
            }

            existingOrderItem.Quantity += 1;
            _context.OrderItems.Update(existingOrderItem);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DecreaseOrderItemQuantity(int orderItemId)
        {
            var existingOrderItem = await _context.OrderItems.FindAsync(orderItemId);
            if (existingOrderItem == null)
            {
                return false;
            }

            existingOrderItem.Quantity -= 1;
            if(existingOrderItem.Quantity == 0)
            {
                await DeleteOrderItem(orderItemId);
            }

            _context.OrderItems.Update(existingOrderItem);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
