using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineStore.Api.Controllers;
using OnlineStore.Domain.Transactions.Commands.AddTransaction;
using OnlineStore.Domain.Transactions.Commands.UpdateTransaction;
using OnlineStore.Domain.Transactions.Queries.GetAllTransactions;
using System.Security.Claims;

namespace OnlineStore.Tests.Controllers;
public class TransactionsControllerTests
{
    [Fact]
    public async Task GetAll_ShouldReturnOkResult()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var controller = new TransactionsController(mediatorMock.Object);

        // Act
        var result = await controller.GetAll(CancellationToken.None);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        mediatorMock.Verify(x => x.Send(It.IsAny<GetAllTransactionsQuery>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Add_ValidData_ShouldReturnOkResult()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var controller = new TransactionsController(mediatorMock.Object);

        // Set up a mock user for the controller
        var userIdClaim = new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString());
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { userIdClaim }));
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var addTransactionData = new AddTransactionData(Guid.NewGuid(), 10);

        // Act
        var result = await controller.Add(addTransactionData);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        mediatorMock.Verify(x => x.Send(It.IsAny<AddTransactionCommand>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Update_ValidData_ShouldReturnNoContentResult()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var controller = new TransactionsController(mediatorMock.Object);

        var transactionId = Guid.NewGuid();
        var isAccepted = true;  // Set as needed

        // Set up the mediator mock to return true for a successful update
        mediatorMock.Setup(x => x.Send(It.IsAny<UpdateTransactionCommand>(), CancellationToken.None))
                    .ReturnsAsync(true);

        // Act
        var result = await controller.Update(transactionId, isAccepted);

        // Assert
        Assert.NotNull(result);  // Ensure the result is not null
        Assert.IsType<NoContentResult>(result);  // Ensure it's NoContentResult

        // Additional check for the response status code
        var statusCode = (result as StatusCodeResult)?.StatusCode;
        Assert.False(statusCode == 404, $"Expected status code other than 404, got {statusCode}");

        mediatorMock.Verify(x => x.Send(It.IsAny<UpdateTransactionCommand>(), CancellationToken.None), Times.Once);
    }



    [Fact]
    public async Task Update_UnsuccessfulUpdate_ShouldReturnNotFoundResult()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var controller = new TransactionsController(mediatorMock.Object);

        var transactionId = Guid.NewGuid();
        var isAccepted = true;  // Set as needed

        mediatorMock.Setup(x => x.Send(It.IsAny<UpdateTransactionCommand>(), CancellationToken.None))
                    .ReturnsAsync(false);

        // Act
        var result = await controller.Update(transactionId, isAccepted);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        mediatorMock.Verify(x => x.Send(It.IsAny<UpdateTransactionCommand>(), CancellationToken.None), Times.Once);
    }


}
