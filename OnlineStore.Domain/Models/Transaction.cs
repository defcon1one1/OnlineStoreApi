namespace OnlineStore.Domain.Models;

public class Transaction
{
    public Guid TransactionId { get; private set; }
    public Guid ProductId { get; private set; }
    public Guid CustomerId { get; private set; }
    public decimal OriginalPrice { get; private set; }
    public decimal CustomerOffer { get; private set; }
    public TransactionStatus Status { get; private set; } = TransactionStatus.Pending;

    public Transaction(
        Guid id,
        Guid productId,
        decimal customerOffer,
        Guid customerId,
        decimal originalPrice)
    {
        TransactionId = id;
        ProductId = productId;
        CustomerId = customerId;
        OriginalPrice = originalPrice;
        CustomerOffer = ValidateOffer(customerOffer);
    }
    public Transaction(
        Guid id,
        Guid productId,
        decimal customerOffer,
        Guid customerId,
        TransactionStatus status,
        decimal originalPrice)
    {
        TransactionId = id;
        ProductId = productId;
        CustomerId = customerId;
        OriginalPrice = originalPrice;
        Status = status;
        CustomerOffer = ValidateOffer(customerOffer);
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