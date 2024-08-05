using Common.Enums;

namespace Domain.Entities;

public class TaskEntity : AuditableEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public TaskEntityStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime DueDate { get; set; }
    public int ProjectId { get; set; }
    public ProjectEntity Project { get; set; } = null!;
    public ICollection<TaskCommentEntity> Comments = [];
    public ICollection<TaskHistoryEntity> Histories = [];
}