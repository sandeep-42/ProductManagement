using AutoMapper;
using Moq;
using ProductManagementAPI.Data;
using ProductManagementAPI.Data.Entities;
using ProductManagementAPI.Data.Repositories;
using ProductManagementAPI.Models;
using ProductManagementAPI.Services;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProductManagementAPI.Tests.Data.Repositories
{
    public class ProductRepositoryTests
    {
        private readonly Mock<IProductDbContext> _mockProductDbContext;
        private readonly Mock<DbSet<Product>> _mockDbSet;
        private readonly ProductRepository _mockProductRepository;

        public ProductRepositoryTests()
        {
            _mockProductDbContext = new Mock<IProductDbContext>();
            _mockDbSet = new Mock<DbSet<Product>>();

            _mockProductDbContext.Setup(x => x.Products).Returns(_mockDbSet.Object);

            _mockProductRepository = new ProductRepository(_mockProductDbContext.Object);
        }

        [Fact]
        public async Task GetAllProductsAsync()
        {
            // Arrange
            var products = new List<Product>()
            {
                new Product() { Id = 1, Name = "Test", Description = "test", StockAvailability = 1, Price = 10M },
                new Product() { Id = 1, Name = "Test", Description = "test", StockAvailability = 1, Price = 10M },
            }.AsQueryable();

            _mockDbSet.As<IQueryable<Product>>().Setup((IQueryable<Product> m) => m.Provider).Returns(products.Provider);
            _mockDbSet.As<IQueryable<Product>>().Setup((IQueryable<Product> m) => m.Expression).Returns(products.Expression);
            _mockDbSet.As<IQueryable<Product>>().Setup((IQueryable<Product> m) => m.ElementType).Returns(products.ElementType);
            _mockDbSet.As<IQueryable<Product>>().Setup((IQueryable<Product> m) => m.GetEnumerator()).Returns(products.GetEnumerator());

            // Act
            var result = await _mockProductRepository.GetAllProductsAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, p => p.Name == "Test");
        }

        [Fact]
        public async Task GetProductByIdAsync()
        {
            // Arrange
            var product = new Product()
            {
                Id = 1,
                Name = "Test",
                Description = "test",
                StockAvailability = 1,
                Price = 10M
            };
            var products = new List<Product>() { product }.AsQueryable();

            _mockDbSet.As<IQueryable<Product>>().Setup((IQueryable<Product> m) => m.Provider).Returns(products.Provider);
            _mockDbSet.As<IQueryable<Product>>().Setup((IQueryable<Product> m) => m.Expression).Returns(products.Expression);
            _mockDbSet.As<IQueryable<Product>>().Setup((IQueryable<Product> m) => m.ElementType).Returns(products.ElementType);
            _mockDbSet.As<IQueryable<Product>>().Setup((IQueryable<Product> m) => m.GetEnumerator()).Returns(products.GetEnumerator());

            // Act
            var result = await _mockProductRepository.GetProductByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test", result.Name);
        }

        [Fact]
        public async Task CreateProductAsync()
        {
            // Arrange
            var product = new Product()
            {
                Id = 1,
                Name = "Test",
                Description = "test",
                StockAvailability = 1,
                Price = 10M
            };

            // Act
            await _mockProductRepository.CreateProductAsync(product);

            // Assert
            _mockDbSet.Verify(x => x.AddAsync(product, default), Times.Once());
        }

        [Fact]
        public void DeleteProduct()
        {
            // Arrange
            var product = new Product()
            {
                Id = 1,
                Name = "Test",
                Description = "test",
                StockAvailability = 1,
                Price = 10M
            };

            // Act
            _mockProductRepository.DeleteProduct(product);

            // Assert
            _mockDbSet.Verify(x => x.Remove(product), Times.Once());
        }

    }

}
