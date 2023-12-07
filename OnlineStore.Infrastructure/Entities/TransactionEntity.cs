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
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
    public ProductEntity Product { get; set; } = new();
    public UserEntity User { get; set; } = new();

    public Transaction ToTransaction()
    {
        return new Transaction(TransactionId, ProductId, CustomerOffer, CustomerId, Status, OriginalPrice);
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
            Status = transaction.Status
        };
    }
}

