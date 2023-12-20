using EvaExchange.API.Data.Entities;
using EvaExchange.API.Data.Repositories;
using EvaExchange.API.Extensions;
using MediatR;

namespace EvaExchange.API.Application.Users.Queries;

public record GetMeSharesQuery : IRequest<IEnumerable<Share>>;

public class GetMeSharesQueryHandler(IShareRepository shareRepository, IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetMeSharesQuery, IEnumerable<Share>>
{
    public Task<IEnumerable<Share>> Handle(GetMeSharesQuery request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext!.User.GetUserId();
        return shareRepository.GetByUserIdAsync(userId);
    }
}