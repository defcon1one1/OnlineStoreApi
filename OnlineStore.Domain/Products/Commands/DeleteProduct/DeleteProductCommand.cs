using MediatR;
using OnlineStore.Domain.Models;
using OnlineStore.Domain.Interfaces.Repositories;

namespace OnlineStore.Domain.Products.Commands.DeleteProduct;
public record DeleteProductCommand(Guid Id) : IRequest<bool>;

internal class DeleteroductCommandHandler(IProductRepository productRepository) : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        Product? productToDelete = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (productToDelete is null) return false; // result NotFound will be returned to client from controller when false
        await _productRepository.DeleteAsync(request.Id);
        return true;
    }
}
