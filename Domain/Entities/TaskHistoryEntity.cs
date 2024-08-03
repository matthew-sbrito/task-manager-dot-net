namespace Domain.Entities;

public class TaskHistoryEntity : AuditableEntity
{
    public int TaskId { get; set; }
    public TaskEntity Task { get; set; } = null!;
    public string Details { get; set; } = null!;
}