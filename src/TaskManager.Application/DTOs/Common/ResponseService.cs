using Microsoft.AspNetCore.Http;

namespace TaskManager.Application.DTOs.Common;

public static class ResponseService
{
    public static Response<T> Success<T>(T data, int statusCode = StatusCodes.Status200OK)
    {
        return new Response<T>
        {
            StatusCode = statusCode,
            Data = data,
            Succeeded = true
        };
    }
    
    public static Response<T> Error<T>(string message, int statusCode = StatusCodes.Status400BadRequest)
    {
        return new Response<T>
        {
            Message = message,
            StatusCode = statusCode,
            Succeeded = false
        };
    }
}