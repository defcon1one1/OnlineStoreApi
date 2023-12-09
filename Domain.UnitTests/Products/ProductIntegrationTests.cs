using FluentAssertions;
using Moq;
using OnlineStore.Domain.Interfaces.Repositories;
using OnlineStore.Domain.Models;
using OnlineStore.Infrastructure.Exceptions;

namespace Domain.UnitTests.Products;
public class ProductIntegrationTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;

    public ProductIntegrationTests()
    {
        _productRepositoryMock = new();
    }
    [Fact]
    public async Task Add_Should_ReturnValidGuidWhenCorrectData()
    {
        // Arrange
        var product = new Product(Guid.NewGuid(), "test", "test", 1);
        _productRepositoryMock.Setup(x => x.AddAsync(product)).Returns(Task.FromResult(product.Id));

        // Act
        var retrievedId = await _productRepositoryMock.Object.AddAsync(product);

        // Assert
        retrievedId.Should().Be(product.Id);

    }
    [Fact]
    public async Task GetById_Should_ReturnProductWithCorrectData()
    {
        // Arrange
        var expectedProduct = new Product(Guid.NewGuid(), "test", "test", 1);
        _productRepositoryMock.Setup(x => x.GetByIdAsync(expectedProduct.Id, It.IsAny<CancellationToken>())).ReturnsAsync(expectedProduct);

        // Act
        var retrievedProduct = await _productRepositoryMock.Object.GetByIdAsync(expectedProduct.Id, It.IsAny<CancellationToken>());

        // Assert
        retrievedProduct.Should().NotBeNull();
        retrievedProduct.Should().BeEquivalentTo(expectedProduct);
    }
    [Fact]
    public async Task NameExists_Should_ReturnTrueWhenNameExists()
    {
        // Arrange
        var product = new Product(Guid.NewGuid(), "test", "test", 1);
        _productRepositoryMock.Setup(x => x.NameExistsAsync(product.Name)).ReturnsAsync(true);

        // Act
        var result = await _productRepositoryMock.Object.NameExistsAsync(product.Name);

        // Assert
        result.Should().BeTrue();
    }
    [Fact]
    public async Task GetAllAsync_Should_ReturnEmptyListWhenNoProductsMatchSearchPhrase()
    {
        // Arrange
        string searchPhrase = "NonExistentProduct";
        _productRepositoryMock.Setup(x => x.GetAllAsync(searchPhrase, CancellationToken.None))
                              .ReturnsAsync(new List<Product>());

        // Act
        var result = await _productRepositoryMock.Object.GetAllAsync(searchPhrase, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task DeleteAsync_Should_ThrowExceptionWhenNoProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _productRepositoryMock.Setup(x => x.GetByIdAsync(productId, It.IsAny<CancellationToken>())).ReturnsAsync((Product)null);
        _productRepositoryMock.Setup(x => x.DeleteAsync(productId))
                         .ThrowsAsync(new DatabaseOperationException("Delete operation failed: product not found."));
        // Act
        Func<Task> act = async () => await _productRepositoryMock.Object.DeleteAsync(productId);

        // Assert
        await act.Should().ThrowAsync<DatabaseOperationException>()
                  .WithMessage("Delete operation failed: product not found.");
    }




    [Fact]
    public async Task UpdateAsync_Should_UpdateProductData()
    {
        // Arrange
        var id = Guid.NewGuid();
        var existingProduct = new Product(id, "OriginalName", "OriginalDescription", 1);
        var updatedProduct = new Product(id, "UpdatedName", "UpdatedDescription", 2);

        _productRepositoryMock.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(existingProduct);
        _productRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Product>())).Callback<Product>(p => existingProduct = p).Returns(Task.CompletedTask);

        // Act
        await _productRepositoryMock.Object.UpdateAsync(updatedProduct);

        // Assert
        existingProduct.Should().BeEquivalentTo(updatedProduct);
    }

}
