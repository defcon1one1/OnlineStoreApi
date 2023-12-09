using FluentAssertions;
using OnlineStore.Domain.Models;

namespace OnlineStore.Tests.Transactions;
public class TransactionUnitTests
{
    [Fact]
    public void Add_Should_ThrowArgumentExceptionWhenOfferNotGreaterThanZero()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        decimal offer = 0;

        // Act & Assert
        Action act = () => new Transaction(id, id, offer, id, 100);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Offer must be greater than 0 and less than original price.");
    }
    [Fact]
    public void Add_Should_ThrowArgumentExceptionWhenOfferGreaterThanOriginalPrice()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        decimal offer = 200;
        decimal originalPrice = 100;

        // Act & Assert
        Action act = () => new Transaction(id, id, offer, id, originalPrice);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Offer must be greater than 0 and less than original price.");
    }
}
