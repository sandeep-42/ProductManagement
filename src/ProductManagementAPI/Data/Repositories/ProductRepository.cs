using Microsoft.EntityFrameworkCore;
using ProductManagementAPI.Data.Entities;
using ProductManagementAPI.Models;

namespace ProductManagementAPI.Data.Repositories
{
    internal class ProductRepository : IProductRepository
    {
        private readonly IProductDbContext _productDbContext;

        public ProductRepository(IProductDbContext productDbContext)
        {
            _productDbContext = productDbContext;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productDbContext.Products.ToListAsync();
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            var productEntity = await _productDbContext.Products.AddAsync(product);
            return productEntity.Entity;
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _productDbContext.Products.SingleOrDefaultAsync(p => p.Id == productId);
        }

        public void DeleteProduct(Product product)
        {
            _productDbContext.Products.Remove(product);
        }
    }
}
