using FluentAssertions;
using Moq;
using OnlineStore.Domain.Interfaces.Repositories;
using OnlineStore.Domain.Models;

namespace Domain.UnitTests.Transactions;

public class TransactionIntegrationTests
{
    private readonly Mock<ITransactionRepository> _transactionRepositoryMock = new();
    [Fact]
    public async Task Add_Should_ReturnValidGuidWhenCorrectData()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        var transaction = new Transaction(id, id, id, 100.0m, 150.0m);
        _transactionRepositoryMock.Setup(x => x.AddAsync(transaction)).Returns(Task.FromResult(transaction.TransactionId));

        // Act
        var retrievedId = await _transactionRepositoryMock.Object.AddAsync(transaction);

        // Assert
        retrievedId.Should().Be(transaction.TransactionId);
    }
    [Fact]
    public async Task GetById_Should_ReturnTransactionWithCorrectData()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        var expectedTransaction = new Transaction(id, id, id, 100, 150);
        _transactionRepositoryMock.Setup(x => x.GetByIdAsync(expectedTransaction.TransactionId, It.IsAny<CancellationToken>())).ReturnsAsync(expectedTransaction);

        // Act
        var retrievedTransaction = await _transactionRepositoryMock.Object.GetByIdAsync(expectedTransaction.TransactionId, It.IsAny<CancellationToken>());

        // Assert
        retrievedTransaction.Should().NotBeNull();
        retrievedTransaction.Should().BeEquivalentTo(expectedTransaction);
    }
    [Fact]
    public async Task Reject_Should_SetStatusForClosedWhen3Revisions()
    {
        // Arrange
        var id = Guid.NewGuid();
        var existingTransaction = new Transaction(id, id, id, 100, 150, 3, TransactionStatus.Pending);
        _transactionRepositoryMock.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(existingTransaction);

        _transactionRepositoryMock
            .Setup(x => x.RejectAsync(id))
            .Callback<Guid>(asyncTransactionId =>
            {
                existingTransaction.SetStatus(TransactionStatus.Closed);
            });

        // Act
        await _transactionRepositoryMock.Object.RejectAsync(id);

        // Assert
        existingTransaction.Status.Should().Be(TransactionStatus.Closed);
    }
    [Fact]
    public async Task Accept_Should_SetStatusForAccepted()
    {
        // Arrange
        var id = Guid.NewGuid();
        var existingTransaction = new Transaction(id, id, id, 100, 150, 3, TransactionStatus.Pending);
        _transactionRepositoryMock.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(existingTransaction);

        _transactionRepositoryMock
            .Setup(x => x.AcceptAsync(id))
            .Callback<Guid>(asyncTransactionId =>
            {
                existingTransaction.SetStatus(TransactionStatus.Accepted);
            });

        // Act
        await _transactionRepositoryMock.Object.AcceptAsync(id);

        // Assert
        existingTransaction.Status.Should().Be(TransactionStatus.Accepted);
    }

    [Fact]
    public async Task Reject_Should_SetStatusForRejectedWhenLessThan3Revisions()
    {
        // Arrange
        var id = Guid.NewGuid();
        var existingTransaction = new Transaction(id, id, id, 100, 150, 2, TransactionStatus.Pending);
        _transactionRepositoryMock.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(existingTransaction);

        _transactionRepositoryMock
            .Setup(x => x.RejectAsync(id))
            .Callback<Guid>(asyncTransactionId =>
            {
                existingTransaction.SetStatus(TransactionStatus.Rejected);
            });

        // Act
        await _transactionRepositoryMock.Object.RejectAsync(id);

        // Assert
        existingTransaction.Status.Should().Be(TransactionStatus.Rejected);
    }

    [Fact]
    public async Task Revise_Should_ChangeData()
    {
        // Arrange
        var id = Guid.NewGuid();
        decimal newOffer = 120.0m;
        var existingTransaction = new Transaction(id, id, id, 100, 150, 2, TransactionStatus.Rejected);
        _transactionRepositoryMock.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(existingTransaction);

        _transactionRepositoryMock
            .Setup(x => x.ReviseAsync(id, newOffer))
            .Callback<Guid, decimal>((asyncTransactionId, asyncNewOffer) =>
            {
                existingTransaction.SetCustomerOffer(asyncNewOffer);
                existingTransaction.SetStatus(TransactionStatus.Pending);
            });


        // Act
        await _transactionRepositoryMock.Object.ReviseAsync(id, newOffer);

        // Assert
        existingTransaction.Status.Should().Be(TransactionStatus.Pending);
        existingTransaction.CustomerOffer.Should().Be(newOffer);
    }

}