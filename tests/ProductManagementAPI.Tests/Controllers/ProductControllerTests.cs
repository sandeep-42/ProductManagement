using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductManagementAPI.Controllers;
using ProductManagementAPI.Models;
using ProductManagementAPI.Services;

namespace ProductManagementAPI.Tests.Controllers
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _mockProductService;

        public ProductControllerTests()
        {
            _mockProductService = new Mock<IProductService>();
        }

        [Fact]
        public async Task GetAllProducts()
        {
            // Arrange
            var controller = new ProductController(_mockProductService.Object);

            // Act
            var result = await controller.GetAllProducts();

            // Assert
            result.Should().NotBeNull();
            _mockProductService.Verify(x => x.GetAllProductsAsync(), Times.Once());
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetProductById()
        {
            // Arrange
            int productId = 1;
            var controller = new ProductController(_mockProductService.Object);

            // Act
            var result = await controller.GetProductById(productId);

            // Assert
            result.Should().NotBeNull();
            _mockProductService.Verify(x => x.GetProductByIdAsync(It.IsAny<int>()), Times.Once());
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task CreateProduct()
        {
            // Arrange
            var productModel = new ProductModel()
            {
                ProductId = 123456,
                ProductName = "sample test",
                ProductDescription = "test",
                StockAvailability = 10,
                Price = 25M,
            };

            var controller = new ProductController(_mockProductService.Object);

            // Act
            var result = await controller.CreateProduct(productModel);

            // Assert
            result.Should().NotBeNull();
            _mockProductService.Verify(x => x.CreateProductAsync(It.IsAny<ProductModel>()), Times.Once());
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task UpdateProduct()
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

            var controller = new ProductController(_mockProductService.Object);

            // Act
            var result = await controller.UpdateProduct(productId, productModel);

            // Assert
            result.Should().NotBeNull();
            _mockProductService.Verify(x => x.UpdateProductByProductIdAsync(It.IsAny<int>(), It.IsAny<ProductModel>()), Times.Once());
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task DeleteProduct()
        {
            // Arrange
            int productId = 1;
            var controller = new ProductController(_mockProductService.Object);

            // Act
            var result = await controller.DeleteProduct(productId);

            // Assert
            _mockProductService.Verify(x => x.DeleteProductByIdAsync(It.IsAny<int>()), Times.Once());
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task IncrementProductQuantity()
        {
            // Arrange
            int productId = 1;
            int quantityId = 5;
            var controller = new ProductController(_mockProductService.Object);

            // Act
            var result = await controller.IncrementProductQuantity(productId, quantityId);

            // Assert
            _mockProductService.Verify(x => x.UpdateProductQuantityByProductIdAsync(It.IsAny<int>(), It.IsAny<int>(), true), Times.Once());
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task DecrementProductQuantity()
        {
            // Arrange
            int productId = 1;
            int quantityId = 5;
            var controller = new ProductController(_mockProductService.Object);

            // Act
            var result = await controller.DecrementProductQuantity(productId, quantityId);

            // Assert
            _mockProductService.Verify(x => x.UpdateProductQuantityByProductIdAsync(It.IsAny<int>(), It.IsAny<int>(), false), Times.Once());
            result.Should().BeOfType<OkResult>();
        }
    }
}
