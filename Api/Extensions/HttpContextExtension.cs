using System.Text.Json;

namespace Api.Extensions;

public static class HttpContextExtension
{
    public static async Task SendResponseAsync<TResponse>(
        this HttpContext context,
        JsonSerializerOptions jsonSerializerOptions,
        TResponse responseBody
    ) where TResponse : class
    {
        var responseBodyText = JsonSerializer.Serialize(responseBody, jsonSerializerOptions);
        await context.Response.WriteAsync(responseBodyText);
    }
}