using System.Security.Claims;

namespace EvaExchange.API.Extensions;

public static class ClaimsExtension
{
    public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    }
}