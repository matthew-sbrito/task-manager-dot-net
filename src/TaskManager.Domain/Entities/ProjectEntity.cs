namespace TaskManager.Domain.Entities;

public class ProjectEntity : AuditableEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ICollection<TaskEntity> Tasks { get; set; } = [];
}