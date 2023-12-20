using EvaExchange.API.Data;
using EvaExchange.API.Data.Entities;
using MediatR;

namespace EvaExchange.API.Application.Trades.Events;

public record TradeCompletedEvent(
    string UserId,
    string ShareId,
    decimal Rate,
    decimal Price,
    TradeType Type) : INotification;

public class TradeCompletedEventHandler(AppDbContext appDbContext) : INotificationHandler<TradeCompletedEvent>
{
    public Task Handle(TradeCompletedEvent notification, CancellationToken cancellationToken)
    {
        appDbContext.Trades.Add(new Trade
        {
            UserId = notification.UserId,
            ShareId = notification.ShareId,
            Rate = notification.Rate,
            Price = notification.Price,
            Type = notification.Type
        });
        return appDbContext.SaveChangesAsync(cancellationToken);
    }
}