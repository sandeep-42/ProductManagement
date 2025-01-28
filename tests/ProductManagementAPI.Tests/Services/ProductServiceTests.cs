using Moq;
using ProductManagementAPI.Data.Repositories;
using AutoMapper;
using ProductManagementAPI.Data.Entities;
using ProductManagementAPI.Data;
using ProductManagementAPI.Services;
using FluentAssertions;
using ProductManagementAPI.Models;
using Microsoft.AspNetCore.Http;

namespace ProductManagementAPI.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductUnitOfWork> _mockProductUnitOfWork;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _mockProductUnitOfWork = new Mock<IProductUnitOfWork>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockMapper = new Mock<IMapper>();
            _productService = new ProductService(_mockProductUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllProductsAsync()
        {
            // Arrange
            var products = new List<Product>()
            {
                new() { Id = 123456, Name = "Test", Description = "sample desc", Price = 100M, StockAvailability = 5, },
                new() { Id = 123457, Name = "Test1", Description = "sample desc1", Price = 200M, StockAvailability = 6, },
            };

            var productsDto = new List<ProductModel>()
            {
                new() {ProductId = 123456, ProductName = "sample test", ProductDescription = "test", StockAvailability = 10, Price = 25M },
                new() {ProductId = 123457, ProductName = "sample test1", ProductDescription = "test1", StockAvailability = 10, Price = 25M },
            };

            _mockMapper.Setup(x => x.Map<List<ProductModel>>(It.IsAny<List<Product>>())).Returns(productsDto);

            _mockProductRepository.Setup(x => x.GetAllProductsAsync()).ReturnsAsync(products);
            _mockProductUnitOfWork.Setup(x => x.productRepository).Returns(_mockProductRepository.Object);

            // Act
            var result = await _productService.GetAllProductsAsync();

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateProductAsync()
        {
            // Arrange
            var productModel = new ProductModel()
            {
                ProductId = 123456, ProductName = "sample test", ProductDescription = "test", StockAvailability = 10, Price = 25M,
            };

            var productEntity = new Product()
            {
                Id = 123456,
                Name = "Test",
                Description = "sample desc",
                Price = 100M,
                StockAvailability = 5,
            };

            _mockMapper.Setup(x => x.Map<Product>(productModel)).Returns(productEntity);
            _mockProductRepository.Setup(x => x.CreateProductAsync(It.IsAny<Product>())).ReturnsAsync(productEntity);
            _mockProductUnitOfWork.Setup(x => x.SaveChangesAsync());
            _mockProductUnitOfWork.Setup(x => x.productRepository).Returns(_mockProductRepository.Object);
            _mockMapper.Setup(x => x.Map<ProductModel>(productEntity)).Returns(productModel);

            // Act
            var result = await _productService.CreateProductAsync(productModel);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ProductModel>();
        }

        [Fact]
        public async Task GetProductById_InvalidId_ReturnsException()
        {
            // Arrange
            var productId = 0;

            // Act & Assert
            await Assert.ThrowsAsync<BadHttpRequestException>(async () => await _productService.GetProductByIdAsync(productId));
        }

        [Fact]
        public async Task GetProductById_ProductNotExist_ReturnsException()
        {
            // Arrange
            var productId = 1;

            _mockProductRepository.Setup(x => x.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);
            _mockProductUnitOfWork.Setup(x => x.productRepository).Returns(_mockProductRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _productService.GetProductByIdAsync(productId));
        }

        [Fact]
        public async Task GetProductByIdAsync()
        {
            // Arrange
            int productId = 1;

            var productModel = new ProductModel()
            {
                ProductId = 123456,
                ProductName = "sample test",
                ProductDescription = "test",
                StockAvailability = 10,
                Price = 25M,
            };

            var productEntity = new Product()
            {
                Id = 123456,
                Name = "Test",
                Description = "sample desc",
                Price = 100M,
                StockAvailability = 5,
            };

            _mockProductRepository.Setup(x => x.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync(productEntity);
            _mockProductUnitOfWork.Setup(x => x.productRepository).Returns(_mockProductRepository.Object);
            _mockMapper.Setup(x => x.Map<ProductModel>(productEntity)).Returns(productModel);

            // Act
            var result = await _productService.GetProductByIdAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProductModel>(result);
        }

        [Fact]
        public async Task UpdateProductByProductIdAsync()
        {
            // Arrange
            int productId = 1;

            var productModel = new ProductModel()
            {
                ProductId = 123456,
                ProductName = "sample test",
                ProductDescription = "test",
                StockAvailability = 10,
                Price = 25M,
            };

            var productEntity = new Product()
            {
                Id = 123456,
                Name = "Test",
                Description = "sample desc",
                Price = 100M,
                StockAvailability = 5,
            };

            _mockMapper.Setup(x => x.Map<Product>(productModel)).Returns(productEntity);
            _mockProductRepository.Setup(x => x.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync(productEntity);
            _mockProductUnitOfWork.Setup(x => x.SaveChangesAsync());
            _mockProductUnitOfWork.Setup(x => x.productRepository).Returns(_mockProductRepository.Object);
            _mockMapper.Setup(x => x.Map<ProductModel>(productEntity)).Returns(productModel);

            // Act
            var result = await _productService.UpdateProductByProductIdAsync(productId, productModel);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProductModel>(result);
        }

        [Fact]
        public async Task DeleteProductByIdAsync()
        {
            // Arrange
            int productId = 1;

            var productEntity = new Product()
            {
                Id = 123456,
                Name = "Test",
                Description = "sample desc",
                Price = 100M,
                StockAvailability = 5,
            };

            _mockProductRepository.Setup(x => x.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync(productEntity);
            _mockProductRepository.Setup(x => x.DeleteProduct(productEntity));
            _mockProductUnitOfWork.Setup(x => x.SaveChangesAsync());
            _mockProductUnitOfWork.Setup(x => x.productRepository).Returns(_mockProductRepository.Object);

            // Act
            await _productService.DeleteProductByIdAsync(productId);

            // Assert
            _mockProductUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [Fact]
        public async Task UpdateProductQuantityByProductIdAsync()
        {
            // Arrange
            int productId = 1;
            int quantity = 1;

            var productEntity = new Product()
            {
                Id = 123456,
                Name = "Test",
                Description = "sample desc",
                Price = 100M,
                StockAvailability = 5,
            };

            _mockProductRepository.Setup(x => x.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync(productEntity);
            _mockProductUnitOfWork.Setup(x => x.SaveChangesAsync());
            _mockProductUnitOfWork.Setup(x => x.productRepository).Returns(_mockProductRepository.Object);

            // Act
            await _productService.UpdateProductQuantityByProductIdAsync(productId, quantity);

            // Assert
            _mockProductUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once());
        }
    }
}
