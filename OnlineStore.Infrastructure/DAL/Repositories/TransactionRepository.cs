using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Models;
using OnlineStore.Domain.Repositories;
using OnlineStore.Infrastructure.Entities;
using OnlineStore.Infrastructure.Exceptions;

namespace OnlineStore.Infrastructure.DAL.Repositories;
public class TransactionRepository(AppDbContext dbContext) : ITransactionRepository
{
    private readonly AppDbContext _dbContext = dbContext;
    public async Task<List<Transaction>> GetAllAsync()
    {
        List<TransactionEntity> transactionEntities = await _dbContext.Transactions.ToListAsync();
        return transactionEntities.Select(transactionEntity => transactionEntity.ToTransaction()).ToList();
    }
    public async Task<List<Transaction>> GetByStatusAsync(TransactionStatus status)
    {
        List<TransactionEntity> transactionEntities = await _dbContext.Transactions.Where(t => t.Status == status).ToListAsync();
        return transactionEntities.Select(t => t.ToTransaction()).ToList();
    }

    public async Task<Transaction?> GetByIdAsync(Guid id)
    {
        TransactionEntity? transactionEntity = await GetEntityByIdAsync(id);
        return transactionEntity?.ToTransaction();
    }
    public async Task AddAsync(Transaction transaction)
    {
        TransactionEntity transactionEntity = TransactionEntity.FromTransaction(transaction);

        try
        {
            await _dbContext.Transactions.AddAsync(transactionEntity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new DatabaseOperationException($"Add transaction operation failed: {ex.Message}");
        }
    }
    public async Task AcceptAsync(Guid transactionId)
    {
        TransactionEntity transactionEntity = await GetEntityByIdAsync(transactionId) ?? throw new DatabaseOperationException("Accept operation failed: transaction not found.");
        if (transactionEntity.Status == TransactionStatus.Pending) throw new DatabaseOperationException("Reject operation failed: transaction is currently not pending.");
        transactionEntity.Status = TransactionStatus.Accepted;
        _dbContext.Transactions.Update(transactionEntity);
        await _dbContext.SaveChangesAsync();
    }
    public async Task RejectAsync(Guid transactionId)
    {
        TransactionEntity transactionEntity = await GetEntityByIdAsync(transactionId) ?? throw new DatabaseOperationException("Reject operation failed: transaction not found.");
        if (transactionEntity.Status == TransactionStatus.Pending) throw new DatabaseOperationException("Reject operation failed: transaction is currently not pending.");
        transactionEntity.Status = transactionEntity.Revisions <= 3 ? TransactionStatus.Rejected : TransactionStatus.Closed;
        _dbContext.Transactions.Update(transactionEntity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task ReviseAsync(Guid transactionId, decimal price)
    {
        TransactionEntity transactionEntity = await GetEntityByIdAsync(transactionId) ?? throw new DatabaseOperationException("Revise operation failed: transaction not found.");
        if (transactionEntity.Status == TransactionStatus.Pending) throw new DatabaseOperationException("Revise operation failed: transaction is currently pending.");
        transactionEntity.Revisions++;
        transactionEntity.Status = TransactionStatus.Pending;
        _dbContext.Update(transactionEntity);
        await _dbContext.SaveChangesAsync();
    }

    private async Task<TransactionEntity?> GetEntityByIdAsync(Guid id)
    {
        return await _dbContext.Transactions.FindAsync(id);
    }
}