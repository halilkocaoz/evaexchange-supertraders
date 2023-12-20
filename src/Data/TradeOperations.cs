using EvaExchange.API.Data.Entities;
using EvaExchange.API.Data.Repositories;
using EvaExchange.API.Infrastructure;

namespace EvaExchange.API.Data;

public interface ITradeOperations
{
    Task<UserShares> BuyAsync(string userId, string shareId, decimal buyingRate);
    Task<UserShares> SellAsync(string userId, string shareId, decimal sellingRate);
}

public class TradeOperations(
    IUserRepository userRepository,
    IShareRepository shareRepository,
    IUserShareRepository userShareRepository) : ITradeOperations
{
    private static readonly object TradingLock = new();
    private static readonly object BalancingLock = new();

    public async Task<UserShares> BuyAsync(string userId, string shareId, decimal buyingRate)
    {
        buyingRate = Math.Round(buyingRate, 2, MidpointRounding.ToZero);
        
        var share = await shareRepository.GetByIdAsync(shareId);
        if (share is null)
            throw new ApiException(400, "Share does not exist");
        
        var user = await userRepository.GetByIdAsync(userId);
        user.ThrowIfInsufficientFunds(buyingRate, share.Price);
        
        var userShare = await userShareRepository.GetPortfolioAsync(userId, shareId);
        var availableShareRate = await GetAvailableRateAsync();

        lock (TradingLock)
        {
            if (userShare == null) // buy new share
            {
                userShare = new UserShares(userId, shareId, availableShareRate, buyingRate, share);
                userShareRepository.AddAsync(userShare).GetAwaiter().GetResult();
            }
            else // buy more rate of the share
            {
                userShare.Buy(availableShareRate, buyingRate, share);
                userShare.Share = null;
                userShareRepository.UpdateAsync(userShare).GetAwaiter().GetResult();
            }
        }

        lock (BalancingLock)
        {
            user.UpdateBalance(-1 * buyingRate * share.Price);
            userRepository.UpdateAsync(user).GetAwaiter().GetResult();
        }

        return userShare;

        async Task<decimal> GetAvailableRateAsync()
        {
            var onMarkedRate = await userShareRepository.GetOnMarkedRateAsync(share.Id);
            return share.Rate - onMarkedRate;
        }
    }

    public async Task<UserShares> SellAsync(string userId, string shareId, decimal sellingRate)
    {
        sellingRate = Math.Round(sellingRate, 2, MidpointRounding.ToZero);

        var userShare = await userShareRepository.GetPortfolioAsync(userId, shareId);
        if (userShare is null)
            throw new ApiException(400, "User does not own this share");

        lock (TradingLock)
        {
            userShare.Sell(sellingRate);
            userShareRepository.UpdateAsync(userShare).GetAwaiter().GetResult();
        }
        
        var user = await userRepository.GetByIdAsync(userId);
        lock (BalancingLock)
        {
            user.UpdateBalance(sellingRate * userShare.Share.Price);
            userRepository.UpdateAsync(user).GetAwaiter().GetResult();
        }

        return userShare;
    }
}