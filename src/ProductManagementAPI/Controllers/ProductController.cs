using Microsoft.AspNetCore.Mvc;
using ProductManagementAPI.Models;
using ProductManagementAPI.Services;

namespace ProductManagementAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductModel productModel)
        {
            var product = await _productService.CreateProductAsync(productModel);
            return Ok(product);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductModel productModel)
        {
            var product = await _productService.UpdateProductByProductIdAsync(id, productModel);
            return Ok(product);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductByIdAsync(id);
            return Ok();
        }

        [HttpPut]
        [Route("add-to-stock/{id}/{quantity}")]
        public async Task<IActionResult> IncrementProductQuantity(int id, int quantity)
        {
            await _productService.UpdateProductQuantityByProductIdAsync(id, quantity);
            return Ok();
        }

        [HttpPut]
        [Route("decrement-stock/{id}/{quantity}")]
        public async Task<IActionResult> DecrementProductQuantity(int id, int quantity)
        {
            await _productService.UpdateProductQuantityByProductIdAsync(id, quantity, false);
            return Ok();
        }
    }
}
