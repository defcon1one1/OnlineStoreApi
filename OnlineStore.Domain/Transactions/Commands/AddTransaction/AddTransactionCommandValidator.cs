using FluentValidation;
using OnlineStore.Domain.Models;
using OnlineStore.Domain.Repositories;
using OnlineStore.Domain.Transactions.Commands.AddTransaction;

internal class AddTransactionCommandValidator : AbstractValidator<AddTransactionCommand>
{
    private readonly IProductRepository _productRepository;

    public AddTransactionCommandValidator(IProductRepository productRepository)
    {
        _productRepository = productRepository;

        RuleFor(c => c.AddTransactionData.ProductId)
            .MustAsync(ProductExists).WithMessage("Product with this ID does not exist.");
    }

    private async Task<bool> ProductExists(Guid productId, CancellationToken cancellationToken)
    {
        Product? product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        return product is not null;
    }
}
