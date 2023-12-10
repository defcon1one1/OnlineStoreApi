using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineStore.Api.Controllers;
using OnlineStore.Domain.Services;
using OnlineStore.Domain.Users.Commands.LoginCommand;

namespace OnlineStore.Tests.Controllers;
public class UsersControllerTests
{
    [Fact]
    public async Task Login_ValidUser_ReturnsOk()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var passwordHasherMock = new Mock<IPasswordHasherService>();
        var controller = new UsersController(mediatorMock.Object, passwordHasherMock.Object);

        mediatorMock.Setup(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new LoginResponse(true, default, default));

        // Act
        IActionResult result = await controller.Login(new LoginRequest("test", "test"));

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
}
