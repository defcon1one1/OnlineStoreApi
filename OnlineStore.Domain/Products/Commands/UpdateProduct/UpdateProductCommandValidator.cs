using FluentValidation;
using OnlineStore.Domain.Repositories;

namespace OnlineStore.Domain.Products.Commands.UpdateProduct;
internal class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly decimal _minimumPrice;

    public UpdateProductCommandValidator(IProductRepository productRepository)
    {
        _minimumPrice = 0.1m; // set the minimum price in constructor
        _productRepository = productRepository;

        RuleFor(c => c.UpdateProductData.Price).GreaterThanOrEqualTo(_minimumPrice).WithMessage($"Price must be greater than {_minimumPrice}.");

        RuleFor(c => c.UpdateProductData.Name).NotEmpty().WithMessage("Name must not be empty.");

        RuleFor(c => c.UpdateProductData.Name).MustAsync(async (name, canellationToken) =>
            !await _productRepository.NameExistsAsync(name))
            .WithMessage("Product with this name already exists.");

        RuleFor(c => c.UpdateProductData).NotNull().WithMessage("Invalid request.").WithErrorCode("404");
    }
}