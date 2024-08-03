using Common.Enums;

namespace Application.DTOs.Response;

public class TaskResponseDto : AuditableResponseDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public TaskEntityStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime DueDate { get; set; }
    public int ProjectId { get; set; }
}