using FluentValidation;
using OnlineStore.Domain.Interfaces.Repositories;

namespace OnlineStore.Domain.Products.Commands.DeleteProduct;
internal class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandValidator(IProductRepository productRepository)
    {
        _productRepository = productRepository;

        RuleFor(c => c.Id)
            .MustAsync(ProductExists)
            .WithMessage("Product with the specified ID does not exist.");
    }

    private async Task<bool> ProductExists(Guid id, CancellationToken cancellationToken)
    {
        return await _productRepository.GetByIdAsync(id, cancellationToken) is not null;
    }
}
