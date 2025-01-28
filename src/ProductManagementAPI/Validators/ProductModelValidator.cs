using FluentValidation;
using ProductManagementAPI.Models;

namespace ProductManagementAPI.Validators
{
    internal class ProductModelValidator : AbstractValidator<ProductModel>
    {
        public ProductModelValidator()
        {
            RuleFor(p => p.ProductName)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");

            When(p => p.ProductDescription != null, () =>
            {
                RuleFor(p => p.ProductDescription)
                .MaximumLength(250).WithMessage("Product descirption must not exceed more than 250 characters.");
            });

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(p => p.StockAvailability)
                .GreaterThan(0).WithMessage("Atleast 1 quantity should be available for product.");
        }
    }
}
