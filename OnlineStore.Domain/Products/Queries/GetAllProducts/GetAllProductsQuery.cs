using MediatR;
using OnlineStore.Domain.Models;
using OnlineStore.Domain.Interfaces.Repositories;

namespace OnlineStore.Domain.Products.Queries.GetAllProducts;
public record GetAllProductsQuery(string SearchPhrase) : IRequest<IReadOnlyCollection<Product>>;

internal class GetAllProductsQueryHandler(IProductRepository productRepository) : IRequestHandler<GetAllProductsQuery, IReadOnlyCollection<Product>>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<IReadOnlyCollection<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        return await _productRepository.GetAllAsync(request.SearchPhrase, cancellationToken);
    }
}
