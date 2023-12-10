using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Domain.Models;
using OnlineStore.Domain.Transactions.Commands.AddTransaction;
using OnlineStore.Domain.Transactions.Commands.ReviseTransaction;
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

        if (!User.IsInRole("Admin") && !User.IsInRole("Employee") && userId != transaction.CustomerId)
        {
            return NotFound();
        }

        return Ok(transaction);
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
        Guid? createdId = await _mediator.Send(new AddTransactionCommand(addTransactionData, userId), CancellationToken.None);
        return createdId.HasValue ? Ok(createdId.Value) : BadRequest();

    }


    [HttpPatch("{id}")]
    [Authorize(Roles = "Employee,Admin")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromQuery] bool isAccepted)
    {
        bool success = await _mediator.Send(new UpdateTransactionCommand(id, isAccepted));
        return success ? NoContent() : NotFound();
    }


    [HttpPatch("{id}/revise")]
    [Authorize]
    public async Task<IActionResult> Revise([FromRoute] Guid id, [FromBody] decimal offer)
    {
        Transaction? transaction = await _mediator.Send(new ReviseTransactionCommand(id, offer));

        if (transaction is null)
        {
            return NotFound();
        }

        Claim? userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
        {
            return Unauthorized();
        }

        if (transaction?.CustomerId != userId)
        {
            return NotFound();
        }

        return transaction is null ? NotFound() : Ok(transaction);
    }


}
