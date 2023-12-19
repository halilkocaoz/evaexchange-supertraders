namespace EvaExchange.API.Data.Entities;

public class UserShares
{
    public string UserId { get; set; }
    public string ShareId { get; set; }
    public decimal Rate { get; set; }
    
    public User User { get; set; }
    public Share Share { get; set; }
}