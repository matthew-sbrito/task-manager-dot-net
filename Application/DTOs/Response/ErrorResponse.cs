namespace Application.DTOs.Response;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = null!;
    public DateTime Timestamp { get; set; }
    public string? StackTrace { get; set; }
}