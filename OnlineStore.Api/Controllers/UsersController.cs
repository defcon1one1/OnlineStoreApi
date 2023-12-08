using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Domain.Services;
using OnlineStore.Domain.Users.Commands.LoginCommand;
using OnlineStore.Domain.Users.Dtos;
using OnlineStore.Domain.Users.Queries.GetUserById;
using LoginRequest = OnlineStore.Domain.Users.Commands.LoginCommand.LoginRequest;

namespace OnlineStore.Api.Controllers;

[ApiController]
[Route("api/user")]
public class UsersController(IMediator mediator, IPasswordHasherService passwordHasherService) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IPasswordHasherService _passwordHasherService = passwordHasherService;
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(Guid id)
    {
        UserDto? userDto = await _mediator.Send(new GetUserByIdQuery(id));
        return Ok(userDto);
    }
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        LoginRequest hashedRequest = new(loginRequest.Email, _passwordHasherService.GenerateHash(loginRequest.Password));
        LoginResponse loginResponse = await _mediator.Send(new LoginCommand(hashedRequest));
        return loginResponse.Success ? Ok(loginResponse) : Unauthorized();
    }
}
