using MediatR;
using OnlineStore.Domain.Models;
using OnlineStore.Domain.Repositories;
using OnlineStore.Domain.Users.Dtos;

namespace OnlineStore.Domain.Users.Queries.GetUserById;
public record GetUserByIdQuery(Guid Id) : IRequest<UserDto?>;

internal class GetUserByIdQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IUserRepository _userRepository = userRepository;
    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        return UserDto.FromUser(user);
    }
}
