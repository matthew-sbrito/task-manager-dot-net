using TaskManager.Application.DTOs.Common;

namespace TaskManager.Api.Extensions;

public static class ResponseExtension
{
    /// <summary>
    /// Creates a <see cref="IResult"/> based on response
    /// </summary>
    /// <typeparam name="TResult">Type of result that desires be returned.</typeparam>
    /// <param name="result">The item desires be return on response.</param>
    /// <returns>The <see cref="IResult"/> created based on <paramref name="result"/> </returns>
    public static IResult ToHttpResponse<TResult>(this Response<TResult> result)
    {
        if (result is { Succeeded: true, Data: not null })
            return Results.Json(result.Data, statusCode: result.StatusCode);
        
        return string.IsNullOrEmpty(result.Message) 
            ? Results.StatusCode(result.StatusCode) 
            : Results.Json(result.ToErrorResponse(), statusCode: result.StatusCode);
    }

    /// <summary>
    /// Creates a <see cref="IResult"/> based on response
    /// </summary>
    /// <typeparam name="TResult">Type of result that desires be returned.</typeparam>
    /// <param name="result">The item desires be return on response.</param>
    /// <param name="uriGenerator">The generator uri that transform "result" on uri.</param>
    /// <returns>The <see cref="IResult"/> created based on <paramref name="result"/> </returns>
    public static IResult ToCreatedResponse<TResult>(this Response<TResult> result, Func<TResult, string> uriGenerator)
    {
        if (result is { Succeeded: true, Data: not null })
            return Results.Created(uriGenerator(result.Data), result.Data);

        return string.IsNullOrEmpty(result.Message) 
            ? Results.StatusCode(result.StatusCode) 
            : Results.Json(result.ToErrorResponse(), statusCode: result.StatusCode);
    }
}