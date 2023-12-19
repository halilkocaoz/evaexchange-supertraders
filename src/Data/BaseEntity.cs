using System.ComponentModel.DataAnnotations.Schema;
using MediatR;

namespace EvaExchange.API.Data;

public class BaseEntity
{
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    private List<INotification> _domainEvents;

    [NotMapped] public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

    protected void AddDomainEvent(INotification eventItem)
    {
        _domainEvents ??= new List<INotification>();
        _domainEvents.Add(eventItem);
    }

    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }
}