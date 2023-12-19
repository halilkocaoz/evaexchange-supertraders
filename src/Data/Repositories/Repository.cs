using System.Linq.Expressions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EvaExchange.API.Data.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    protected IQueryable<T> GetAsQueryable(Expression<Func<T, bool>> predicate = null);
    Task<T> GetAsync(Expression<Func<T, bool>> predicate);
    Task<T> GetByIdAsync(object id);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
}

public abstract class Repository<T>(AppDbContext dbContext, IPublisher mediatrPublisher, ILogger<T> logger)
    : IRepository<T>
    where T : BaseEntity
{
    private readonly DbSet<T> _table = dbContext.Set<T>();

    public virtual IQueryable<T> GetAsQueryable(Expression<Func<T, bool>> predicate = null)
    {
        return predicate == null
            ? _table.AsQueryable()
            : _table.Where(predicate);
    }

    public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _table.FirstOrDefaultAsync(predicate);
    }

    public virtual async Task<T> GetByIdAsync(object id)
    {
        return await _table.FindAsync(id);
    }

    private async Task PublishEventsAsync(T entity)
    {
        try
        {
            // do not remove this try/catch block - it is required to catch exceptions when publishing events
            // if there is an exception while publishing events, the entity will not be saved to the database and the data will be lost
            logger.LogInformation("Publishing events for entity {entity}", entity);
            if (entity.DomainEvents != null)
            {
                foreach (var domainEvent in entity.DomainEvents)
                {
                    await mediatrPublisher.Publish(domainEvent);
                }

                entity.ClearDomainEvents();
            }
        }
        catch (Exception e)
        {
            logger.Log(LogLevel.Error, e, "Error publishing events for entity {entity}", entity);
        }
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        var publishEventsTask = PublishEventsAsync(entity);

        await _table.AddAsync(entity);
        await dbContext.SaveChangesAsync();

        await publishEventsTask;
        return entity;
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        var publishEventsTask = PublishEventsAsync(entity);

        _table.Update(entity);
        await dbContext.SaveChangesAsync();

        await publishEventsTask;
        return entity;
    }
}