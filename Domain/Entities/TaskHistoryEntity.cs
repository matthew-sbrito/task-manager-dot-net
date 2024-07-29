using Common.Enums;

namespace Domain.Entities;

public class TaskHistoryEntity : AuditableEntity
{
    public int TaskId { get; set; }
    public TaskEntity Task { get; set; } = null!;
    public string? Title { get; set; } = null!;
    public string? Description { get; set; } = null!;
    public TaskEntityStatus? Status { get; set; }
    public TaskPriority? Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public string Details { get; set; } = null!;
}