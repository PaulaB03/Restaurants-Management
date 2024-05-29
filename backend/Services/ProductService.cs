using backend.Data;
using backend.Models;
using backend.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> AddProduct(Product product)
        {
            _context.Products.Add(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Product> GetProductById(int productId)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<bool> UpdatePrice(int productId, float price)
        {
            var existingProduct = await _context.Products.FindAsync(productId);
            if (existingProduct == null)
            {
                return false;
            }

            existingProduct.Price = price;
            _context.Products.Update(existingProduct);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddCategory(Category category)
        {
            _context.Categories.Add(category);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Category> GetCategoryById(int categoryId)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
        }

        public async Task<List<Category>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<List<Product>> GetProductsByRestaurant(int restaurantId)
        {
            return await _context.Products.Where(p => p.RestaurantId == restaurantId).ToListAsync();
        }

        public async Task<List<Product>> GetProductsByRestaurantAndCategory(int restaurantId, int categoryId)
        {
            return await _context.Products.Where(p => p.RestaurantId == restaurantId && p.CategoryId == categoryId).ToListAsync();
        }
    }
}
