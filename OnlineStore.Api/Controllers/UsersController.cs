using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Domain.Services;
using OnlineStore.Domain.Users.Commands.LoginCommand;
using LoginRequest = OnlineStore.Domain.Users.Commands.LoginCommand.LoginRequest;

namespace OnlineStore.Api.Controllers;

[ApiController]
[Route("api/user")]
public class UsersController(IMediator mediator, IPasswordHasherService passwordHasherService) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IPasswordHasherService _passwordHasherService = passwordHasherService;
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        LoginRequest hashedRequest = new(loginRequest.Email, _passwordHasherService.GenerateHash(loginRequest.Password));
        LoginResponse loginResponse = await _mediator.Send(new LoginCommand(hashedRequest));
        return loginResponse.Success ? Ok(loginResponse) : Unauthorized();
    }
}
