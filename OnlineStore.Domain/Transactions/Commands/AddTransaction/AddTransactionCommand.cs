using MediatR;
using OnlineStore.Domain.Models;
using OnlineStore.Domain.Repositories;

namespace OnlineStore.Domain.Transactions.Commands.AddTransaction;
public record AddTransactionCommand(AddTransactionData AddTransactionData, Guid UserId) : IRequest<Guid>;

internal class AddTransactionCommandHandler(ITransactionRepository transactionRepository, IProductRepository productRepository) : IRequestHandler<AddTransactionCommand, Guid>
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<Guid> Handle(AddTransactionCommand request, CancellationToken cancellationToken)
    {
        Product? product = await _productRepository.GetByIdAsync(request.AddTransactionData.ProductId, cancellationToken);
        if (product is null) return Guid.Empty;
        Transaction transactionToAdd = new(Guid.NewGuid(), request.AddTransactionData.ProductId, request.AddTransactionData.Offer, request.UserId, product.Price);
        await _transactionRepository.AddAsync(transactionToAdd);
        return transactionToAdd.TransactionId;
    }
}