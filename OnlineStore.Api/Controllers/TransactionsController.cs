using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Domain.Models;
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
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> Add([FromBody] AddTransactionData addTransactionData)
    {
        Claim? userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
        {
            return Unauthorized();
        }
        Guid? transactionId = await _mediator.Send(new AddTransactionCommand(addTransactionData, userId));
        return transactionId.HasValue ? CreatedAtAction(nameof(Add), transactionId) : BadRequest();
    }
    [HttpPatch("{id}")]
    [Authorize(Roles = "Employee,Admin")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromQuery] bool isAccepted)
    {
        bool success = await _mediator.Send(new UpdateTransactionCommand(id, isAccepted));
        return success ? NoContent() : NotFound();
    }

}
