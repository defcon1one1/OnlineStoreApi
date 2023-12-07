using OnlineStore.Domain.Models;

namespace OnlineStore.Infrastructure.Entities;
public class TransactionEntity
{
    public Guid TransactionId { get; set; }
    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal CustomerOffer { get; set; }
    public int Revisions { get; set; }
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;

    public Transaction ToTransaction()
    {
        return new Transaction(TransactionId, ProductId, CustomerOffer, UserId, Status, OriginalPrice);
    }
    public static TransactionEntity FromTransaction(Transaction transaction)
    {
        return new TransactionEntity
        {
            TransactionId = transaction.TransactionId,
            ProductId = transaction.ProductId,
            UserId = transaction.CustomerId,
            OriginalPrice = transaction.OriginalPrice,
            CustomerOffer = transaction.CustomerOffer,
            Status = transaction.Status
        };
    }
}

