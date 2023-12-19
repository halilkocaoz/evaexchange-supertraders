namespace EvaExchange.API.Infrastructure.Models;

public record TokenModel(string Token, DateTime ExpiresInUtc)
{
}
