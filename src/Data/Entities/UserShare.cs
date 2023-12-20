using EvaExchange.API.Application.Trades.Events;
using EvaExchange.API.Infrastructure;

namespace EvaExchange.API.Data.Entities;

public class UserShares : BaseEntity
{
    public UserShares()
    {
    }

    public UserShares(string userId, string shareId, decimal availableShareRate, decimal rate)
    {
        UserId = userId;
        ShareId = shareId;
        Buy(availableShareRate, rate);
    }

    public void Buy(decimal availableShareRate, decimal buyingRate)
    {
        if (availableShareRate < buyingRate)
            throw new ApiException(400, "Buying rate is higher than the available share rate.");

        Rate += buyingRate;
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new TradeCompletedEvent(UserId, ShareId, buyingRate, Share.Price, TradeType.Buy));
    }

    public void Sell(decimal sellingRate)
    {
        if (Rate < sellingRate)
            throw new ApiException(400, "Selling rate is higher than the current rate");

        Rate -= sellingRate;
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new TradeCompletedEvent(UserId, ShareId, sellingRate, Share.Price, TradeType.Sell));
    }

    public string UserId { get; set; }
    public string ShareId { get; set; }
    public decimal Rate { get; set; }

    public User User { get; set; }
    public Share Share { get; set; }
}