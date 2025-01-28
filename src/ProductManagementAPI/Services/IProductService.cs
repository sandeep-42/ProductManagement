using ProductManagementAPI.Models;

namespace ProductManagementAPI.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductModel>> GetAllProductsAsync();

        Task<ProductModel> CreateProductAsync(ProductModel productModel);

        Task<ProductModel> GetProductByIdAsync(int productId);

        Task<ProductModel> UpdateProductByProductIdAsync(int productId, ProductModel productModel);

        Task DeleteProductByIdAsync(int productId);

        Task UpdateProductQuantityByProductIdAsync(int productId, int quantity, bool isIncrement = true);
    }
}
