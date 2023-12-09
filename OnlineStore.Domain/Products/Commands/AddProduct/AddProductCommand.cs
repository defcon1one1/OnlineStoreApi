using MediatR;
using OnlineStore.Domain.Models;
using OnlineStore.Domain.Interfaces.Repositories;

namespace OnlineStore.Domain.Products.Commands.AddProduct;
public record AddProductCommand(AddProductData AddProductData) : IRequest<Guid>;

internal class AddProductCommandHandler(IProductRepository productRepository) : IRequestHandler<AddProductCommand, Guid>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<Guid> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        Product productToAdd = new(Guid.NewGuid(), request.AddProductData.Name, request.AddProductData.Description, request.AddProductData.Price);
        await _productRepository.AddAsync(productToAdd);
        return await Task.FromResult(productToAdd.Id);
    }
}
