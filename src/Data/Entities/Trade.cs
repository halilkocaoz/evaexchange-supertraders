namespace EvaExchange.API.Data.Entities;

public class Trade
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string UserId { get; set; }
    public string ShareId { get; set; }
    public decimal Rate { get; set; }
    public decimal Price { get; set; }
    public TradeType Type { get; set; }
}

public enum TradeType
{
    Buy,
    Sell
}