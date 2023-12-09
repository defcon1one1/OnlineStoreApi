using FluentAssertions;
using OnlineStore.Domain.Models;

namespace Domain.UnitTests.Products;

public class ProductUnitTests
{
    [Fact]
    public void Add_Should_ThrowArgumentExceptionWhenPriceNotGreaterThanZero()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        decimal price = 0m;

        // Act & Assert
        Action act = () => new Product(id, "test", "desc", price);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Price must be grdeater than 0.");
    }
}