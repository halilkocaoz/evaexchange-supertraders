using EvaExchange.API.Data.Entities;
using EvaExchange.API.Data.Repositories;
using EvaExchange.API.Extensions;
using MediatR;

namespace EvaExchange.API.Application.Users.Queries;

public record GetMeQuery : IRequest<User>;

public class GetMeQueryHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetMeQuery, User>
{
    public Task<User> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext!.User.GetUserId();
        return userRepository.GetByIdAsync(userId);
    }
}