using backend.Models;

namespace backend.Services.Interface
{
    public interface IProductService
    {
        Task<bool> AddProduct(Product product);
        Task<Product> GetProductById(int productId);
        Task<bool> UpdatePrice(int productId, float price);
        Task<bool> DeleteProduct(int productId);
        Task<bool> AddCategory(Category category);
        Task<Category> GetCategoryById(int categoryId);
        Task<List<Category>> GetCategories();
    }
}
