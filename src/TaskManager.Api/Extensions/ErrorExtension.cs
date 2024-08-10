using ErrorOr;

namespace TaskManager.Api.Extensions;

public static class ErrorExtension
{
    /// <summary>
    /// Creates a <see cref="IResult"/> based on response
    /// </summary>
    /// <param name="errors">The errors of response.</param>
    /// <returns>The <see cref="IResult"/> created based on <paramref name="errors"/> </returns>
    public static IResult ToProblemDetails(this List<Error> errors)
    {
        var firstError = errors.First();

        return Results.Problem(
            statusCode: GetStatusCode(firstError),
            title: GetTitle(firstError),
            type: GetType(firstError),
            extensions: new Dictionary<string, object?>
            {
                { "errors", errors }
            });
    }

    private static int GetStatusCode(Error error)
    {
        if (error.Code.Contains("Forbidden", StringComparison.InvariantCultureIgnoreCase))
        {
            return StatusCodes.Status403Forbidden;
        }

        return error.Type switch
        {
            ErrorType.Failure => StatusCodes.Status400BadRequest,
            ErrorType.Unexpected => StatusCodes.Status400BadRequest,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };
    }
    
    private static string GetTitle(Error error)
    {
        if (error.Code.Contains("Forbidden", StringComparison.InvariantCultureIgnoreCase))
        {
            return "Forbidden";
        }

        return error.Type switch
        {
            ErrorType.Failure => "Bad Request",
            ErrorType.Unexpected => "Bad Request",
            ErrorType.Validation => "Bad Request",
            ErrorType.Conflict => "Conflict",
            ErrorType.NotFound => "Not Found",
            ErrorType.Unauthorized => "Unauthorized",
            _ => "Internal Server Error"
        };
    }

    private static string GetType(Error error)
    {
        if (error.Code.Contains("Forbidden", StringComparison.InvariantCultureIgnoreCase))
        {
            return "https://tools.ietf.org/html/rfc7231#section-6.5.3";
        }

        return error.Type switch
        {
            ErrorType.Failure => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            ErrorType.Unexpected => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
            ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            ErrorType.Unauthorized => "https://tools.ietf.org/html/rfc7231#section-3.1",
            _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };
    }
}