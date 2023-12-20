using EvaExchange.API.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EvaExchange.API.Data.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetByEmailAsync(string email);
}

public class UserRepository(AppDbContext dbContext, IPublisher mediatrPublisher, ILogger<User> logger)
    : Repository<User>(dbContext, mediatrPublisher, logger), IUserRepository
{
    public Task<User> GetByEmailAsync(string email)
    {
        return GetAsQueryable().FirstOrDefaultAsync(u => u.Email == email);
    }
}