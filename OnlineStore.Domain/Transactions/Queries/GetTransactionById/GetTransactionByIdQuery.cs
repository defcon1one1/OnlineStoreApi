using MediatR;
using OnlineStore.Domain.Models;
using OnlineStore.Domain.Repositories;

namespace OnlineStore.Domain.Transactions.Queries.GetTransactionById;
public record GetTransactionByIdQuery(Guid Id) : IRequest<Transaction?>;

internal class GetTransactionByIdQueryHandler(ITransactionRepository transactionRepository) : IRequestHandler<GetTransactionByIdQuery, Transaction?>
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    public async Task<Transaction?> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        Transaction? transaction = await _transactionRepository.GetByIdAsync(request.Id, cancellationToken);
        return transaction;
    }
}