using EvaExchange.API.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EvaExchange.API.Data.Repositories;

public interface IShareRepository : IRepository<Share>
{
    Task<List<Share>> GetAllAsync();
}

public class ShareRepository(AppDbContext dbContext, IPublisher mediatrPublisher, ILogger<Share> logger) :
    Repository<Share>(dbContext, mediatrPublisher, logger), IShareRepository
{
    public Task<List<Share>> GetAllAsync()
    {
        return GetAsQueryable().ToListAsync();
    }
}
