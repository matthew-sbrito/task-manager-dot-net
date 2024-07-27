using Common.Enums;

namespace Domain.Entities;

public class TaskEntity : AuditableEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int ProjectId { get; set; }
    public ProjectEntity Project { get; set; } = null!;
    public TaskEntityStatus TaskStatus { get; set; }
    public DateTime DueDate { get; set; }
}