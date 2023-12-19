namespace EvaExchange.API.Infrastructure;

public class ApiException(int statusCode, string message, int? code = null) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
    public int? Code { get; } = code;
}