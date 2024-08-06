using TaskManager.Application.DTOs.Response;
using TaskManager.Shared.Helpers;

namespace TaskManager.Application.DTOs.Common;

public class Response<TData>
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public TData? Data { get; set; }
    public bool Succeeded { get; set; }

    public ErrorResponse ToErrorResponse()
    {
        return new ErrorResponse
        {
            StatusCode = StatusCode,
            Message = Message ?? string.Empty,
            Timestamp = DateTimeHelper.UtcNow()
        };
    }
}