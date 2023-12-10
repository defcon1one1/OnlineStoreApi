using OnlineStore.Domain.Models;

namespace OnlineStore.Infrastructure.Entities;
public class TransactionEntity
{
    public Guid TransactionId { get; set; }
    public Guid ProductId { get; set; }
    public Guid CustomerId { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal CustomerOffer { get; set; }
    public int Revisions { get; set; }
    public TransactionStatus Status { get; set; }

    public Transaction ToTransaction()
    {
        return new Transaction(TransactionId, ProductId, CustomerId, CustomerOffer, OriginalPrice, Revisions, Status);
    }
    public static TransactionEntity FromTransaction(Transaction transaction)
    {
        return new TransactionEntity
        {
            TransactionId = transaction.TransactionId,
            ProductId = transaction.ProductId,
            CustomerId = transaction.CustomerId,
            OriginalPrice = transaction.OriginalPrice,
            CustomerOffer = transaction.CustomerOffer,
            Revisions = transaction.Revisions,
            Status = transaction.Status
        };
    }
}

