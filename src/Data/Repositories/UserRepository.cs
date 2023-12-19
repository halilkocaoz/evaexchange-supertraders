using EvaExchange.API.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EvaExchange.API.Data.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetByEmailAsync(string email);
}

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext dbContext, IPublisher mediatrPublisher, ILogger<User> logger) :
        base(dbContext, mediatrPublisher, logger)
    {
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await GetAsQueryable().FirstOrDefaultAsync(u => u.Email == email);
    }
}