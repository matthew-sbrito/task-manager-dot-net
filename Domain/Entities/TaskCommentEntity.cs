namespace Domain.Entities;

public class TaskCommentEntity : AuditableEntity
{
    public string Content { get; set; } = null!;
}