using MediatR;
using OnlineStore.Domain.Interfaces.Repositories;
using OnlineStore.Domain.Models;

namespace OnlineStore.Domain.Transactions.Commands.ReviseTransaction;
public record ReviseTransactionCommand(Guid TransactionId, decimal Offer) : IRequest<Transaction?>;

public class ReviseTransactionCommandHandler(ITransactionRepository transactionRepository) : IRequestHandler<ReviseTransactionCommand, Transaction?>
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    public async Task<Transaction?> Handle(ReviseTransactionCommand request, CancellationToken cancellationToken)
    {
        Transaction? transactionToRevise = await _transactionRepository.GetByIdAsync(request.TransactionId, cancellationToken);
        if (transactionToRevise is null) return null;

        await _transactionRepository.ReviseAsync(request.TransactionId, request.Offer);
    }
}