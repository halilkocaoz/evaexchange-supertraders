using EvaExchange.API.Data.Entities;
using EvaExchange.API.Data.Repositories;
using EvaExchange.API.Extensions;
using MediatR;

namespace EvaExchange.API.Application.Users.Queries;

public record GetMePortfolioQuery : IRequest<IEnumerable<UserShares>>;

public class GetMePortfolioQueryHandler(IUserShareRepository userShareRepository, IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetMePortfolioQuery, IEnumerable<UserShares>>
{
    public Task<IEnumerable<UserShares>> Handle(GetMePortfolioQuery request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext!.User.GetUserId();
        return userShareRepository.GetPortfolioAsync(userId);
    }
}