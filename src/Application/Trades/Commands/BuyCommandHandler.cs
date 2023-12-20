using EvaExchange.API.Data;
using EvaExchange.API.Data.Entities;
using EvaExchange.API.Extensions;
using MediatR;

namespace EvaExchange.API.Application.Trades.Commands;

public record BuyCommand(string ShareId, decimal Rate) : IRequest<UserShares>;

public class BuyCommandHandler(ITradeOperations tradeOperations, IHttpContextAccessor httpContextAccessor) : IRequestHandler<BuyCommand, UserShares>
{
    public Task<UserShares> Handle(BuyCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext!.User.GetUserId();
        return tradeOperations.BuyAsync(userId, request.ShareId, request.Rate);
    }
}