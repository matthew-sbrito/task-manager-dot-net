namespace Application.DTOs.Response;

public class TaskCommentResponseDto : AuditableResponseDto
{
    public string Content { get; set; } = null!;
    public int TaskId { get; set; }
}