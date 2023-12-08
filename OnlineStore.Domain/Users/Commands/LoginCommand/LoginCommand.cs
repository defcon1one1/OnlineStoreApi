﻿using MediatR;
using OnlineStore.Domain.Models;
using OnlineStore.Domain.Repositories;
using OnlineStore.Domain.Services;

namespace OnlineStore.Domain.Users.Commands.LoginCommand;
public record LoginCommand(LoginRequest LoginRequest) : IRequest<LoginResponse>;

public class LoginCommandHandler
    (IUserRepository userRepository, IJwtService jwtService)
    : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IJwtService _jwtService = jwtService;
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        bool success = _userRepository.VerifyLogin(request.LoginRequest.Email, request.LoginRequest.Password, out Guid id);

        if (!success)
        {
            return await Task.FromResult(new LoginResponse(false));
        }
        else
        {
            User? user = await _userRepository.GetByIdAsync(id);
            string? token = _jwtService.GenerateJwtToken(user);
            return await Task.FromResult(new LoginResponse(true, id, token));
        }
    }
}
