using FluentValidation.TestHelper;
using ProductManagementAPI.Data.Entities;
using ProductManagementAPI.Models;
using ProductManagementAPI.Validators;

namespace ProductManagementAPI.Tests.Validators
{
    public class ProductModelValidatorTests
    {
        private readonly ProductModelValidator _productValidator;

        public ProductModelValidatorTests()
        {
            _productValidator = new ProductModelValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var result = _productValidator.TestValidate(new ProductModel() { ProductName = "" });
            result.ShouldHaveValidationErrorFor(p => p.ProductName);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Valid_Product()
        {
            var result = _productValidator.TestValidate(new ProductModel() { ProductName = "Laptop", Price = 1000, StockAvailability = 1 });
            result.ShouldNotHaveAnyValidationErrors();
        }

    }
}
