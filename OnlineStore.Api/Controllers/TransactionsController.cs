using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Domain.Models;
using OnlineStore.Domain.Products.Queries.GetProductById;
using OnlineStore.Domain.Transactions.Commands.AddTransaction;
using OnlineStore.Domain.Transactions.Commands.UpdateTransaction;
using OnlineStore.Domain.Transactions.Queries.GetAllTransactions;
using OnlineStore.Domain.Transactions.Queries.GetTransactionById;
using System.Security.Claims;

namespace OnlineStore.Api.Controllers;

[Route("api/transaction")]
[ApiController]
public class TransactionsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly decimal _minimumOfferToOriginalPriceRatio = 0.5m; // set minimum price offer ratio

    [HttpGet]
    [Authorize(Roles = "Employee,Admin")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        IReadOnlyCollection<Transaction> transactions = await _mediator.Send(new GetAllTransactionsQuery(), cancellationToken);
        return Ok(transactions);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        Claim? userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
        {
            return Unauthorized();
        }

        Transaction? transaction = await _mediator.Send(new GetTransactionByIdQuery(id), cancellationToken);
        if (transaction is null)
        {
            return NotFound();
        }


#pragma warning disable CS8602 // Dereference of a possibly null reference.
        if (User.IsInRole("Admin") || User.IsInRole("Employee") || userId == transaction.CustomerId)
        {
            return Ok(transaction);
        }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        return Forbid();
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Add([FromBody] AddTransactionData addTransactionData)
    {
        Claim? userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
        {
            return Unauthorized();
        }
        Product? product = await _mediator.Send(new GetProductByIdQuery(addTransactionData.ProductId));

        if (product is null) return NotFound();

        decimal minimumPrice = product.Price * _minimumOfferToOriginalPriceRatio;
        if (addTransactionData.Offer < minimumPrice)
        {
            return BadRequest($"Price for this product must be at least {minimumPrice}.");
        }
        if (addTransactionData.Offer > product.Price)
        {
            return BadRequest($"Transaction offer cannot be higher than the original price ({product.Price}).");
        }

        Transaction transactionToAdd = new(Guid.NewGuid(), product.Id, addTransactionData.Offer, userId, product.Price);
        return CreatedAtAction(nameof(Add), transactionToAdd.TransactionId);

    }
    [HttpPatch("{id}")]
    [Authorize(Roles = "Employee,Admin")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromQuery] bool isAccepted)
    {
        bool success = await _mediator.Send(new UpdateTransactionCommand(id, isAccepted));
        return success ? NoContent() : NotFound();
    }
    [HttpPatch("{id}")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> Revise([FromRoute] Guid id, [FromBody] decimal offer)
    {

    }

}
