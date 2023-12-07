using MediatR;
using OnlineStore.Domain.Models;
using OnlineStore.Domain.Repositories;

namespace OnlineStore.Domain.Products.Queries.GetProductById;
public record GetProductByIdQuery(Guid Id) : IRequest<Product>;

internal class GetProductByIdQueryHandler(IProductRepository productRepository) : IRequestHandler<GetProductByIdQuery, Product>
{
    private readonly IProductRepository _productRepository = productRepository;
    public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        Product? product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
#pragma warning disable CS8603 // handled by the controller
        return product;
#pragma warning restore CS8603 // handled by the controller
    }
}