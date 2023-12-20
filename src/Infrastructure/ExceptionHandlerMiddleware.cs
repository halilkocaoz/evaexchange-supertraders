using FluentValidation;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace EvaExchange.API.Infrastructure;

public class ExceptionHandlerMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context, ILogger<ExceptionHandlerMiddleware> logger)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException exception)
        {
            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                message = exception.Message,
                errors = exception.Errors,
                code = 400,
            }));
        }
        catch (ApiException exception)
        {
            context.Response.StatusCode = exception.StatusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                message = exception.Message,
                code = exception.Code ?? exception.StatusCode,
            }));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unhandled exception.");
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                message = "Unhandled exception. System have been notified.",
            }));
        }
    }
}