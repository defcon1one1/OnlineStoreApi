using MediatR;
using OnlineStore.Domain.Models;
using OnlineStore.Domain.Repositories;

namespace OnlineStore.Domain.Transactions.Commands.UpdateTransaction;
public record UpdateTransactionCommand(Guid TransactionId, bool IsAccepted) : IRequest<bool>;

public class UpdateTransactionCommandHandler(ITransactionRepository transactionRepository) : IRequestHandler<UpdateTransactionCommand, bool>
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    public async Task<bool> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        Transaction? transactionToUpdate = await _transactionRepository.GetByIdAsync(request.TransactionId, cancellationToken);
        if (transactionToUpdate is null) return false;

        if (request.IsAccepted) await _transactionRepository.AcceptAsync(request.TransactionId);
        else await _transactionRepository.RejectAsync(request.TransactionId);

        return true;
    }
}
