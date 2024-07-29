using System.Text.Json;
using Api.Extensions;
using Application.DTOs.Response;
using Application.Exceptions;
using Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Middlewares;

public class HandleExceptionMiddleware(IOptions<JsonOptions> options, ILogger<HandleExceptionMiddleware> logger) : IMiddleware
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = options
        .Value
        .JsonSerializerOptions;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }
    
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var message = exception.Message;
        var stackTrace = exception.StackTrace;

        logger.LogError("Action error message: {Message}", message);
        logger.LogError("Action error stackTrace: {StackTrace}", stackTrace);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = GetStatusCodeByException(exception);

        var responseBody = new ErrorResponse
        {
            StatusCode = context.Response.StatusCode,
            Message = exception.Message,
            Timestamp = DateTimeHelper.UtcNow(),
            StackTrace = $"{message} ${stackTrace}"
        };

        await context.SendResponseAsync(_jsonSerializerOptions, responseBody);
    }

    private static int GetStatusCodeByException(Exception exception)
    {
        return exception switch
        {
            UnauthorizedException => StatusCodes.Status401Unauthorized,
            ForbiddenException => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}