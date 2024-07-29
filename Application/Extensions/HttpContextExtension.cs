using Microsoft.AspNetCore.Http;
using Application.Exceptions;

namespace Application.Extensions;

public static class HttpContextExtension
{
    public static int GetUserId(this HttpContext httpContext)
    {
        var hasHeader = httpContext.Request.Headers
            .TryGetValue("x-user-id", out var userIdValue);

        if (hasHeader && int.TryParse(userIdValue, out var userId))
            return userId;
        
        throw new UnauthorizedException("User ID not found in headers");
    }
}