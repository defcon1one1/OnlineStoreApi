using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Domain.Models;
using OnlineStore.Domain.Products.Commands.AddProduct;
using OnlineStore.Domain.Products.Commands.DeleteProduct;
using OnlineStore.Domain.Products.Commands.UpdateProduct;
using OnlineStore.Domain.Products.Queries.GetProductById;
using OnlineStore.Domain.Products.Queries.GetProductsQuery;
using OnlineStore.Domain.Services;

namespace OnlineStore.Api.Controllers;

[Route("api/product")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IJwtService _jwtService;

    public ProductsController(IMediator mediator, IJwtService jwtService)
    {
        _mediator = mediator;
        _jwtService = jwtService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        IReadOnlyCollection<Product> products = await _mediator.Send(new GetAllProductsQuery(), cancellationToken);
        return Ok(products);
    }

    [HttpGet]
    [Authorize]
    [Route("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        Product? product = await _mediator.Send(new GetProductByIdQuery(id), cancellationToken);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    [Authorize(Roles = "Employee,Admin")]
    public async Task<IActionResult> Add([FromBody] AddProductData addProductData)
    {
        Guid? guid = await _mediator.Send(new AddProductCommand(addProductData));
        return guid.HasValue ? Created() : BadRequest();
    }

    [HttpPut]
    [Authorize(Roles = "Employee,Admin")]
    [Route("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductData updateProductData)
    {
        bool updated = await _mediator.Send(new UpdateProductCommand(id, updateProductData));
        return updated ? NoContent() : NotFound(); // command returns false when the product of {id} was not found in the database
    }

    [HttpDelete]
    [Authorize(Roles = "Employee,Admin")]
    [Route("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        bool deleted = await _mediator.Send(new DeleteProductCommand(id));
        return deleted ? NoContent() : NotFound(); // command returns false when the product of {id} was not found in the database
    }
}
