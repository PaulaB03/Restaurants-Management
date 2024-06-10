using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Models;
using backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
    [TestClass]
    public class OrderServiceTests
    {
        private const string ConnectionString = "Server=DESKTOP-FHLTERG\\SQLEXPRESS;Database=yummy6;Trusted_Connection=True;TrustServerCertificate=True";

        private DataContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseSqlServer(ConnectionString)
                .Options;
            return new DataContext(options);
        }

        [TestInitialize]
        public void Initialize()
        {
            using var context = GetDbContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [TestMethod]
        public async Task GetOrdersAsync_ShouldReturnOrders_ForSpecificUser()
        {
            // Arrange
            var context = GetDbContext();
            var service = new OrderService(context);

            var userId = "877fb976-4162-465c-ae94-49dba5521e64";
            context.Orders.AddRange(new List<Order>
            {
                new Order { OrderId = 1, UserId = userId, OrderItems = new List<OrderItem>() },
                new Order { OrderId = 2, UserId = userId, OrderItems = new List<OrderItem>() }
            });
            await context.SaveChangesAsync();

            var orders = await service.GetOrdersAsync(userId);

            Assert.AreEqual(2, orders.Count());
            Assert.IsTrue(orders.All(o => o.UserId == userId));
        }

        [TestMethod]
        public async Task GetOrderByIdAsync_ShouldReturnOrder_ForSpecificUserAndOrderId()
        {
            // Arrange
            var context = GetDbContext();
            var service = new OrderService(context);

            var userId = "877fb976-4162-465c-ae94-49dba5521e64";
            var order = new Order { OrderId = 1, UserId = userId, OrderItems = new List<OrderItem>() };
            context.Orders.Add(order);
            await context.SaveChangesAsync();

            // Act
            var result = await service.GetOrderByIdAsync(order.OrderId, userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(order.OrderId, result.OrderId);
            Assert.AreEqual(order.UserId, result.UserId);
        }

        [TestMethod]
        public async Task AddOrderAsync_ShouldAddOrder_AndCalculateTotalPrice()
        {
            // Arrange
            var context = GetDbContext();
            var service = new OrderService(context);

            var product = new Product { Id = 1, Price = 10 };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var order = new Order
            {
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = product.Id, Quantity = 2 }
                }
            };

            // Act
            var result = await service.AddOrderAsync(order);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(20, result.TotalPrice);
            Assert.AreEqual(1, context.Orders.Count());
        }

        [TestMethod]
        public async Task DeleteOrderAsync_ShouldDeleteOrder_IfExists()
        {
            // Arrange
            var context = GetDbContext();
            var service = new OrderService(context);

            var order = new Order { OrderId = 1, OrderItems = new List<OrderItem>() };
            context.Orders.Add(order);
            await context.SaveChangesAsync();

            // Act
            var result = await service.DeleteOrderAsync(order.OrderId);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(0, context.Orders.Count());
        }

        [TestMethod]
        public async Task DeleteOrderAsync_ShouldReturnFalse_IfOrderDoesNotExist()
        {
            // Arrange
            var context = GetDbContext();
            var service = new OrderService(context);

            // Act
            var result = await service.DeleteOrderAsync(999);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
