using backend.Models;
using backend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            var result = await _productService.AddProduct(product);
            if (result)
            {
                return Ok(product);
            }
            return BadRequest("Failed to add product!");
        }

        [HttpGet("products/{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var product = await _productService.GetProductById(productId);  
            if (product == null)
            {
                return NotFound($"Product with id {productId} not found!");
            }
            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{productId}/{price}")]
        public async Task<IActionResult> UpdatePrice(int productId, float price)
        {
            var result = await _productService.UpdatePrice(productId, price);   
            if (result)
            {
                return Ok("Price updated");
            }
            return BadRequest("Failed to update price");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var result = await _productService.DeleteProduct(productId);
            if (result)
            {
                return Ok("Product deleted");
            }
            return BadRequest("Failed to delete product!");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory(Category category)
        {
            var result = await _productService.AddCategory(category);
            if (result)
            {
                return Ok(category);
            }
            return BadRequest("Failed to add category!");
        }

        [HttpGet("categories/{categoryId}")]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            var category = await _productService.GetCategoryById(categoryId);
            if (category == null)
            {
                return NotFound($"Category with id {categoryId} not found!");
            }
            return Ok(category);
        }

        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            var categoriesList = await _productService.GetCategories();
            return Ok(categoriesList);
        }
    }
}
