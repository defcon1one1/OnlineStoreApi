using FluentValidation;
using OnlineStore.Domain.Repositories;

namespace OnlineStore.Domain.Products.Commands.AddProduct;

internal class AddProductCommandValidator : AbstractValidator<AddProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly decimal _minimumPrice;

    public AddProductCommandValidator(IProductRepository productRepository)
    {
        _minimumPrice = 0.1m; // set the minimum price in constructor
        _productRepository = productRepository;

        RuleFor(c => c.AddProductData.Price).GreaterThan(_minimumPrice)
            .WithMessage($"Price must be greater than {_minimumPrice}.");

        RuleFor(c => c.AddProductData.Name).NotEmpty()
            .WithMessage("Name must not be empty.");

        RuleFor(c => c.AddProductData.Name).MustAsync(async (name, canellationToken) =>
            !await _productRepository.NameExistsAsync(name))
            .WithMessage("Product with this name already exists.");

        RuleFor(c => c.AddProductData).NotNull().WithMessage("Invalid request.");
    }
}
