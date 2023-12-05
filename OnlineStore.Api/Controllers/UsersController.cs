using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        return Ok();
    }
    [HttpPost]
    public async Task<IActionResult> Logout([FromBody] LoginRequest loginRequest)
    {
        return Ok();
    }
}
