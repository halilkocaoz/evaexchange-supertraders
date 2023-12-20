using EvaExchange.API.Infrastructure;

namespace EvaExchange.API.Data.Entities;

public class Share : BaseEntity
{
    public Share(string id, decimal rate, decimal price, string creatorUserId)
    {
        Id = id;
        Rate = rate;
        Price = price;
        CreatorUserId = creatorUserId;
    }
    
    public void Update(decimal price, string updaterUserId)
    {
        if (CreatorUserId != updaterUserId)
            throw new ApiException(400, "Cannot update share created by another user.");
        
        if (UpdatedAt.HasValue && UpdatedAt.Value > DateTime.UtcNow.AddHours(-1))
            throw new ApiException(400, "Cannot update more than once per one hour.");
        
        Price = price;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public string Id { get; init; }
    public string CreatorUserId { get; init; }
    public decimal Rate { get; init; }
    public decimal Price { get; private set; }
}