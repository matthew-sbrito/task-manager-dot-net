using ErrorOr;
using Microsoft.AspNetCore.Http;

namespace TaskManager.Infrastructure.Extensions;

public static class HttpContextExtension
{
    public static ErrorOr<int> GetUserId(this HttpContext httpContext)
    {
        var hasHeader = httpContext.Request.Headers
            .TryGetValue("x-user-id", out var userIdValue);

        if (hasHeader && int.TryParse(userIdValue, out var userId))
        {
            return userId;
        }

        return Error.Unauthorized(description: "User ID not found in headers");
    }
}