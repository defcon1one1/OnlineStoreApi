using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Domain.Models;
using OnlineStore.Domain.Products.Commands.AddProduct;
using OnlineStore.Domain.Products.Queries.GetProductById;
using OnlineStore.Domain.Products.Queries.GetProductsQuery;

namespace OnlineStore.Api.Controllers;
[Route("api/product")]
[ApiController]
public class ProductsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        IReadOnlyCollection<Product> products = await _mediator.Send(new GetAllProductsQuery(), cancellationToken);
        return Ok(products);
    }
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        Product product = await _mediator.Send(new GetProductByIdQuery(id), cancellationToken);
        return Ok(product);
    }
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddProductData addProductData)
    {
        Guid guid = await _mediator.Send(new AddProductCommand(addProductData));
        return Ok(guid);
    }
}
