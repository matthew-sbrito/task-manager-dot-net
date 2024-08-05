namespace Domain.Entities;

public class TaskCommentEntity : AuditableEntity
{
    public string Content { get; set; } = null!;
    public int TaskId { get; set; }
    public TaskEntity Task { get; set; } = null!;
}