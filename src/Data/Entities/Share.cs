namespace EvaExchange.API.Data.Entities;

public class Share : BaseEntity
{
    public Share(string id, decimal rate, decimal price)
    {
        Id = id;
        Rate = rate;
        Price = price;
    }
    
    public void Update(decimal price)
    {
        if (UpdatedAt.HasValue && UpdatedAt.Value > DateTime.UtcNow.AddHours(-1))
            throw new Exception("Cannot update more than once per one hour.");
        
        Price = price;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public string Id { get; init; }
    public decimal Rate { get; init; }
    public decimal Price { get; private set; }
}