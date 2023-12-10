using FluentValidation;
using OnlineStore.Domain.Interfaces.Repositories;
using Transaction = OnlineStore.Domain.Models.Transaction;
using TransactionStatus = OnlineStore.Domain.Models.TransactionStatus;

namespace OnlineStore.Domain.Transactions.Commands.ReviseTransaction;
internal class ReviseTransactionCommandValidator : AbstractValidator<ReviseTransactionCommand>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly decimal _minimumOfferToOriginalPriceRatio = 0.5m; // set minimum price ratio
    public ReviseTransactionCommandValidator(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;

        RuleFor(x => x.TransactionId)
            .MustAsync(TransactionExists)
            .WithMessage("Transaction not found.")
            .DependentRules(() =>
            {
                RuleFor(x => x.Offer)
                    .MustAsync(OfferIsValid)
                    .WithMessage("Offer must be lower than the original price and at least {_minimumPriceToOfferRatio * 100}% of the original price.");

                RuleFor(x => x.TransactionId)
                    .MustAsync(TransactionIsPending)
                    .WithMessage("Transaction is not pending or closed.");
            });
    }

    private async Task<bool> TransactionIsPending(Guid id, CancellationToken cancellationToken)
    {
        Transaction? transaction = await _transactionRepository.GetByIdAsync(id, cancellationToken);

        return transaction?.Status == TransactionStatus.Pending;
    }

    private async Task<bool> TransactionExists(Guid id, CancellationToken cancellationToken)
    {
        Transaction? transaction = await _transactionRepository.GetByIdAsync(id, cancellationToken);
        return transaction is not null;
    }

    private async Task<bool> OfferIsValid(ReviseTransactionCommand command, decimal offer, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(command.TransactionId, cancellationToken);

        if (transaction is null)
        {
            return false;
        }

        return offer >= transaction.OriginalPrice * _minimumOfferToOriginalPriceRatio && offer <= transaction.OriginalPrice;
    }
}
