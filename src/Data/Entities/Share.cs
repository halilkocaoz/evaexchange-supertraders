namespace EvaExchange.API.Data.Entities;

public class Share : BaseEntity
{
    public string Id { get; set; }
    
    public decimal Rate { get; set; }
    public decimal Price { get; set; }
}