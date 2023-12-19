using System.Text.Json;
using MediatR;

namespace EvaExchange.API.Infrastructure.Application;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling command {Command}", request);
        var response = await next();
        logger.LogInformation("Command {Command} handled - response: {@Response}", request, JsonSerializer.Serialize(response));
        return response;
    }
}