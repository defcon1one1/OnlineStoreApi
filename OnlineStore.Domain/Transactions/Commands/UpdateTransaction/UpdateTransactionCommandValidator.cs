using FluentValidation;
using OnlineStore.Domain.Interfaces.Repositories;
using OnlineStore.Domain.Models;

namespace OnlineStore.Domain.Transactions.Commands.UpdateTransaction;
internal class UpdateTransactionCommandValidator : AbstractValidator<UpdateTransactionCommand>
{
    private readonly ITransactionRepository _transactionRepository;
    public UpdateTransactionCommandValidator(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;

        RuleFor(x => x.TransactionId)
            .MustAsync(TransactionExists)
            .WithMessage("Transaction not found.")
            .DependentRules(() =>
                        {

                            RuleFor(x => x.TransactionId)
                                .MustAsync(TransactionIsPending)
                                .WithMessage("Transaction is not pending or closed.");
                        });
    }

    private async Task<bool> TransactionExists(Guid id, CancellationToken cancellationToken)
    {
        Transaction? transaction = await _transactionRepository.GetByIdAsync(id, cancellationToken);
        return transaction is not null;
    }
    private async Task<bool> TransactionIsPending(Guid id, CancellationToken cancellationToken)
    {
        Transaction? transaction = await _transactionRepository.GetByIdAsync(id, cancellationToken);

        return transaction?.Status == TransactionStatus.Pending;
    }
}
