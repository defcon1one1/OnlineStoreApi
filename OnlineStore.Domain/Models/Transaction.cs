namespace OnlineStore.Domain.Models;

public class Transaction
{
    public Guid TransactionId { get; private set; }
    public Guid ProductId { get; private set; }
    public Guid CustomerId { get; private set; }
    public decimal CustomerOffer { get; private set; }
    public int Revisions { get; private set; }
    public TransactionStatus Status { get; private set; } = TransactionStatus.Pending;
    public decimal OriginalPrice { get; private set; }

    public Transaction(Guid transactionId,
        Guid productId,
        Guid customerId,
        decimal customerOffer,
        decimal originalPrice)
    {
        TransactionId = transactionId;
        ProductId = productId;
        CustomerId = customerId;
        OriginalPrice = originalPrice;
        CustomerOffer = ValidateOffer(customerOffer);
    }

    public Transaction(
        Guid transactionId,
        Guid productId,
        Guid customerId,
        decimal customerOffer,
        decimal originalPrice,
        int revisions,
        TransactionStatus status
)
    {
        TransactionId = transactionId;
        ProductId = productId;
        CustomerId = customerId;
        OriginalPrice = originalPrice;
        CustomerOffer = ValidateOffer(customerOffer);
        Revisions = revisions;
        Status = status;
    }

    public void SetStatus(TransactionStatus status)
    {
        Status = status;
    }
    public void SetCustomerOffer(decimal offer)
    {
        CustomerOffer = offer;
    }

    private decimal ValidateOffer(decimal offer)
    {
        if (offer <= 0 || offer >= OriginalPrice)
        {
            throw new ArgumentException("Offer must be greater than 0 and less than original price.");
        }
        return offer;
    }
}

public enum TransactionStatus
{
    Pending,
    Accepted,
    Rejected,
    Closed
}