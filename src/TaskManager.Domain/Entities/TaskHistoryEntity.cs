namespace TaskManager.Domain.Entities;

public class TaskHistoryEntity : AuditableEntity
{
    public string Details { get; set; } = null!;
    public int TaskId { get; set; }
    public TaskEntity Task { get; set; } = null!;
}