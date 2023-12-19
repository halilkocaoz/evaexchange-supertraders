namespace EvaExchange.API.Data.Entities;

public class User : BaseEntity
{
    public User(string email, string password)
    {
        Id = Guid.NewGuid().ToString();
        Email = email;
        // todo: hash password
        Password = password;
    }
    
    public bool PasswordMatches(string password)
    {
        // todo: compare hashed passwords
        return Password == password;
    }
    
    public string Id { get; init; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    
    public IEnumerable<UserShares> Shares { get; set; }
}