using MediatR;
using OnlineStore.Domain.Models;
using OnlineStore.Domain.Interfaces.Repositories;

namespace OnlineStore.Domain.Products.Commands.UpdateProduct;
public record UpdateProductCommand(Guid Id, UpdateProductData UpdateProductData) : IRequest<bool>;

public class UpdateProductCommandHandler(IProductRepository productRepository) : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IProductRepository _productRepository = productRepository;
    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        Product? productToUpdate = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (productToUpdate is null) return false; // result NotFound will be returned to client from controller when false
        productToUpdate.Update(request.UpdateProductData);
        await _productRepository.UpdateAsync(productToUpdate);
        return true;
    }
}
