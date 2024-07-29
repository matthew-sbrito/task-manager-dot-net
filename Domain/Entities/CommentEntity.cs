namespace Domain.Entities;

public class CommentEntity : AuditableEntity
{
    public string Content { get; set; } = null!;
}