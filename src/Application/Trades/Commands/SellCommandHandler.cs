using EvaExchange.API.Data;
using EvaExchange.API.Data.Entities;
using EvaExchange.API.Extensions;
using MediatR;

namespace EvaExchange.API.Application.Trades.Commands;

public record SellCommand(string ShareId, decimal Rate) : IRequest<UserShares>;

public class SellCommandHandler(ITradeOperations tradeOperations, IHttpContextAccessor httpContextAccessor) : IRequestHandler<SellCommand, UserShares>
{
    public Task<UserShares> Handle(SellCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext!.User.GetUserId();
        return tradeOperations.SellAsync(userId, request.ShareId, request.Rate);
    }
}