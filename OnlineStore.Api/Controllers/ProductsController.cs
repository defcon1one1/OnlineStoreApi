﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Domain.Models;
using OnlineStore.Domain.Products.Commands.AddProduct;
using OnlineStore.Domain.Products.Commands.DeleteProduct;
using OnlineStore.Domain.Products.Commands.UpdateProduct;
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
        Product? product = await _mediator.Send(new GetProductByIdQuery(id), cancellationToken);
        if (product is null) return NotFound();
        else return Ok(product);
    }
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddProductData addProductData)
    {
        Guid guid = await _mediator.Send(new AddProductCommand(addProductData));
        return Ok(guid);
    }
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductData updateProductData)
    {
        bool updated = await _mediator.Send(new UpdateProductCommand(id, updateProductData));
        if (updated) return NoContent();
        else return NotFound(); // command returns false when product of {id} was not found in db
    }
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        bool deleted = await _mediator.Send(new DeleteProductCommand(id));
        if (deleted) return NoContent();
        else return NotFound(); // command returns false when product of {id} was not found in db
    }
}
