namespace EvaExchange.API.Models;

public record UserShareResponseModel(
    string Symbol,
    decimal Rate,
    decimal CurrentPrice,
    decimal Total);