using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineStore.Api.Controllers;
using OnlineStore.Domain.Models;
using OnlineStore.Domain.Products.Commands.AddProduct;
using OnlineStore.Domain.Products.Queries.GetAllProducts;

namespace OnlineStore.Tests.Controllers;

public class ProductControllerTests
{
    [Fact]
    public async Task GetAll_ValidData_ShouldReturnOkResult()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var controller = new ProductsController(mediatorMock.Object);

        var expectedProducts = new List<Product>()
        {
            new(Guid.NewGuid(), "test", "test", 10),
            new(Guid.NewGuid(), "test", "test", 10)
        };
        mediatorMock.Setup(x => x.Send(It.IsAny<GetAllProductsQuery>(), CancellationToken.None))
                    .ReturnsAsync(expectedProducts);

        // Act
        var result = await controller.GetAll(searchPhrase: null, cancellationToken: default);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualProducts = Assert.IsAssignableFrom<IReadOnlyCollection<Product>>(okResult.Value);
        Assert.Equal(expectedProducts, actualProducts);

        mediatorMock.Verify(x => x.Send(It.IsAny<GetAllProductsQuery>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Add_ValidData_ShouldReturnCreatedAtActionResult()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var controller = new ProductsController(mediatorMock.Object);

        var addProductData = new AddProductData("test", "test", 10);
        var createdProductId = Guid.NewGuid();

        mediatorMock.Setup(x => x.Send(It.IsAny<AddProductCommand>(), CancellationToken.None))
                    .ReturnsAsync(createdProductId);

        // Act
        var result = await controller.Add(addProductData);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var actualCreatedProductId = Assert.IsType<Guid>(createdAtActionResult.Value);
        Assert.Equal(createdProductId, actualCreatedProductId);

        mediatorMock.Verify(x => x.Send(It.IsAny<AddProductCommand>(), CancellationToken.None), Times.Once);
    }
}

