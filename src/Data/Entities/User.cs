using EvaExchange.API.Infrastructure;

namespace EvaExchange.API.Data.Entities;

public class User : BaseEntity
{
    public User(string email, string password, string fullName)
    {
        Id = Guid.NewGuid().ToString();
        Email = email;
        FullName = fullName;
        // todo: hash password
        Password = password;
        Balance = 1000;
    }
    
    public bool PasswordMatches(string password)
    {
        // todo: compare hashed passwords
        return Password == password;
    }
    
    public void UpdateBalance(decimal amount)
    {
        Balance += amount;
    }
    
    public void ThrowIfInsufficientFunds(decimal buyingRate, decimal price)
    {
        if (Balance < buyingRate * price)
            throw new ApiException(400, "Insufficient funds");
    }
    
    public string Id { get; init; }
    public string Email { get; private set; }
    public string FullName { get; private set; }
    public string Password { get; private set; }
    public decimal Balance { get; private set; }
    
    public IEnumerable<UserShares> Shares { get; set; }
}