using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Domain.Models;
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
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
        {
            return Unauthorized();
        }

        Transaction? transaction = await _mediator.Send(new GetTransactionByIdQuery(id), cancellationToken);
        if (transaction == null)
        {
            return NotFound();
        }


        if (User.IsInRole("Admin") || User.IsInRole("Employee") || userId == transaction.CustomerId)
        {
            return Ok(transaction);
        }

        return Forbid();
    }
}
