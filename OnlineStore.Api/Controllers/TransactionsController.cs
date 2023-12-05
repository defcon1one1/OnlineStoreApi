using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TransactionsController : ControllerBase
{
    [HttpGet]
    [Authorize] // for client
    public async Task<IActionResult> GetLoggedInUserTransactions()
    {
        return Ok();
    }

    [HttpPost]
    [Authorize] // for client
    public async Task<IActionResult> AddTransaction()
    {
        return Ok();
    }

    [HttpPut]
    [Authorize] // for employer
    public async Task<IActionResult> RespondToTransaction()
    {
        return Ok();
    }


    [HttpGet]
    [Authorize] // for employer
    public async Task<IActionResult> GetUserTransactions()
    {
        return Ok();
    }

    [HttpGet]
    [Authorize] // for employer
    public async Task<IActionResult> GetAllPendingTransactions()
    {
        return Ok();
    }
}
