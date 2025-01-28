using AutoMapper;
using ProductManagementAPI.Data;
using ProductManagementAPI.Data.Entities;
using ProductManagementAPI.Models;

namespace ProductManagementAPI.Services
{
    internal class ProductService : IProductService
    {
        private readonly IProductUnitOfWork _productUnitOfWork;
        private readonly IMapper _productMapper;

        public ProductService(IProductUnitOfWork productUnitOfWork, IMapper productMapper)
        {
            _productUnitOfWork = productUnitOfWork;
            _productMapper = productMapper;
        }

        /// <summary>
        /// Get(s) the list of all products.
        /// </summary>
        /// <returns>List of product(s).</returns>
        public async Task<IEnumerable<ProductModel>> GetAllProductsAsync()
        {
            var products = await _productUnitOfWork.productRepository.GetAllProductsAsync();
            var productList = _productMapper.Map<IEnumerable<ProductModel>>(products);

            return productList;
        }

        /// <summary>
        /// Validate(s) and creat(s) a product.
        /// </summary>
        /// <param name="productModel">Product model.</param>
        /// <returns>Product model.</returns>
        public async Task<ProductModel> CreateProductAsync(ProductModel productModel)
        {
            var productEntity = _productMapper.Map<Product>(productModel);
            productEntity.Id = GenerateUniqueNumberForProductId();
            var createdProductEntity = await _productUnitOfWork.productRepository.CreateProductAsync(productEntity);
            await _productUnitOfWork.SaveChangesAsync();

            var productDto = _productMapper.Map<ProductModel>(createdProductEntity);
            return productDto;
        }

        /// <summary>
        /// Get(s) the product based on given product id.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Product model.</returns>
        public async Task<ProductModel> GetProductByIdAsync(int productId)
        {
            var productEntity = await ValidateProductInputParam(productId);

            var productDto = _productMapper.Map<ProductModel>(productEntity);
            return productDto;
        }

        
        /// <summary>
        /// Validate(s) and update(s) the product details.
        /// </summary>
        /// <param name="productId">Product identifier.</param>
        /// <param name="productModel">Product model.</param>
        /// <returns>Product model.</returns>
        public async Task<ProductModel> UpdateProductByProductIdAsync(int productId, ProductModel productModel)
        {
            var productEntity = await ValidateProductInputParam(productId);

            productEntity.Name = productModel.ProductName;
            productEntity.Description = productModel.ProductDescription;
            productEntity.Price = productModel.Price;
            productEntity.StockAvailability = productModel.StockAvailability;
            await _productUnitOfWork.SaveChangesAsync();

            var productDto = _productMapper.Map<ProductModel>(productEntity);
            return productDto;
        }

        /// <summary>
        /// Delete(s) a product.
        /// </summary>
        /// <param name="productId">Product identifier.</param>
        /// <returns>200 Ok</returns>
        public async Task DeleteProductByIdAsync(int productId)
        {
            var productEntity = await ValidateProductInputParam(productId);

            _productUnitOfWork.productRepository.DeleteProduct(productEntity);
            await _productUnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <param name="isIncrement"></param>
        /// <returns></returns>
        /// <exception cref="BadHttpRequestException"></exception>
        public async Task UpdateProductQuantityByProductIdAsync(int productId, int quantity, bool isIncrement = true)
        {
            var productEntity = await ValidateProductInputParam(productId);

            if (quantity <= 0)
            {
                throw new BadHttpRequestException("Quantity value should be greater than 0.");
            }

            productEntity.StockAvailability = isIncrement ? productEntity.StockAvailability + quantity : productEntity.StockAvailability - quantity;
            await _productUnitOfWork.SaveChangesAsync();

        }

        #region Private Methods

        /// <summary>
        /// Validation to check whether given product id is valid & exists in DB.
        /// </summary>
        /// <param name="productId">Product identifier.</param>
        /// <returns>Product Entity.</returns>
        private async Task<Product> ValidateProductInputParam(int productId)
        {
            if (productId <= 0)
            {
                throw new BadHttpRequestException("Product id is invalid.");
            }

            var productEntity = await _productUnitOfWork.productRepository.GetProductByIdAsync(productId) ?? throw new KeyNotFoundException("Product does not exist.");
            return productEntity;
        }

        /// <summary>
        /// Generates a 6-digit unique number for product Id.
        /// </summary>
        /// <returns>Unique number.</returns>
        private int GenerateUniqueNumberForProductId()
        {
            int hashCode = Guid.NewGuid().GetHashCode();

            int uniqueNum = Math.Abs(hashCode) % 900000 + 100000;
            return uniqueNum;
        }

        #endregion
    }
}
