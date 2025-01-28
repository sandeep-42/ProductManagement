using ProductManagementAPI.Data.Entities;

namespace ProductManagementAPI.Data.Repositories
{
    internal interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();

        Task<Product> CreateProductAsync(Product product);

        Task<Product?> GetProductByIdAsync(int productId);

        void DeleteProduct(Product product);
    }
}
