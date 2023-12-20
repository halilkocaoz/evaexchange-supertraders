using EvaExchange.API.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EvaExchange.API.Data.Repositories;

public interface IUserShareRepository : IRepository<UserShares>
{
    Task<IEnumerable<UserShares>> GetPortfolioAsync(string userId);
    Task<UserShares> GetPortfolioAsync(string userId, string shareId);
    Task<decimal> GetOnMarkedRateAsync(string shareId);
}

public class UserShareRepository(AppDbContext dbContext, IPublisher mediatrPublisher, ILogger<UserShares> logger)
    : Repository<UserShares>(dbContext, mediatrPublisher, logger), IUserShareRepository
{
    public async Task<IEnumerable<UserShares>> GetPortfolioAsync(string userId)
    {
        return await GetAsQueryable()
            .Include(x => x.Share)
            .AsNoTracking()
            .Where(x => x.UserId == userId).ToListAsync();
    }

    public async Task<UserShares> GetPortfolioAsync(string userId, string shareId)
    {
        return await GetAsQueryable()
            .Include(x => x.Share)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.ShareId == shareId);
    }

    public Task<decimal> GetOnMarkedRateAsync(string shareId)
    {
        return GetAsQueryable()
            .Where(x => x.ShareId == shareId)
            .SumAsync(x => x.Rate);
    }
}