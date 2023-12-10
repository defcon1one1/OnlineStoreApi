using FluentValidation;
using OnlineStore.Domain.Interfaces.Repositories;
using OnlineStore.Domain.Models;
using OnlineStore.Domain.Transactions.Commands.AddTransaction;

internal class AddTransactionCommandValidator : AbstractValidator<AddTransactionCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly decimal _minimumPriceToOfferRatio = 0.5m;

    public AddTransactionCommandValidator(IProductRepository productRepository)
    {
        _productRepository = productRepository;

        RuleFor(c => c.AddTransactionData.ProductId)
            .MustAsync(BeValidGuid).WithMessage("Product ID is not correct.")
            .MustAsync(ProductExists).WithMessage("Product does not exist.")
            .DependentRules(() =>
            {
                RuleFor(c => c.AddTransactionData.Offer)
                    .MustAsync(OfferIsValid).WithMessage($"Offer must be lower than the original price and at least {_minimumPriceToOfferRatio * 100}% of the original price.");
            });
    }

    private Task<bool> BeValidGuid(Guid guid, CancellationToken token)
    {
        return Task.FromResult(Guid.TryParse(guid.ToString(), out _));
    }

    private async Task<bool> ProductExists(Guid productId, CancellationToken cancellationToken)
    {
        Product? product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        return product is not null;
    }

    private async Task<bool> OfferIsValid(AddTransactionCommand command, decimal offer, CancellationToken cancellationToken)
    {
        Product product = await _productRepository.GetByIdAsync(command.AddTransactionData.ProductId, cancellationToken);
        decimal originalPrice = product.Price;

        return originalPrice > offer && (offer >= originalPrice * _minimumPriceToOfferRatio);
    }
}