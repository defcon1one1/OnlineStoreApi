using MediatR;
using OnlineStore.Domain.Models;
using OnlineStore.Domain.Repositories;

namespace OnlineStore.Domain.Transactions.Queries.GetAllTransactions;
public record GetAllTransactionsQuery() : IRequest<IReadOnlyCollection<Transaction>>;

internal class GetAllTransactionsQueryHandler(ITransactionRepository transactionRepository) : IRequestHandler<GetAllTransactionsQuery, IReadOnlyCollection<Transaction>>
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;

    public async Task<IReadOnlyCollection<Transaction>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
    {
        return await _transactionRepository.GetAllAsync(cancellationToken);
    }
}