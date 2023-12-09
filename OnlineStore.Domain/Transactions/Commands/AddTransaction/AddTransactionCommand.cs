using FluentValidation;
using MediatR;
using OnlineStore.Domain.Models;
using OnlineStore.Domain.Repositories;

namespace OnlineStore.Domain.Transactions.Commands.AddTransaction;

public record AddTransactionCommand(AddTransactionData AddTransactionData, Guid UserId) : IRequest<Guid>;

internal class AddTransactionCommandHandler(ITransactionRepository transactionRepository, IProductRepository productRepository) : IRequestHandler<AddTransactionCommand, Guid>
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly IProductRepository _productRepository = productRepository;
    private readonly decimal _minimumRatio = 0.5m;

    public async Task<Guid> Handle(AddTransactionCommand request, CancellationToken cancellationToken)
    {
        Product? product = await _productRepository.GetByIdAsync(request.AddTransactionData.ProductId, cancellationToken);
        decimal minimumPrice = product.Price * _minimumRatio;
        if (product.Price < request.AddTransactionData.Offer)
        {
            throw new ValidationException("Transaction offer cannot be greater than the original price.");
        }
        if (request.AddTransactionData.Offer < minimumPrice)
        {
            throw new ValidationException($"Transaction offer must be at least {minimumPrice}");
        }
        Transaction transactionToAdd = new(Guid.NewGuid(), product.Id, request.AddTransactionData.Offer, request.UserId, product.Price);
        await _transactionRepository.AddAsync(transactionToAdd);
        return transactionToAdd.TransactionId;
    }
}
