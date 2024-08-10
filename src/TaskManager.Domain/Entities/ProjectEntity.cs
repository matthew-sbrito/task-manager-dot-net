using ErrorOr;

namespace TaskManager.Domain.Entities;

public class ProjectEntity : AuditableEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ICollection<TaskEntity> Tasks { get; set; } = [];

    public ErrorOr<Success> AddTask(TaskEntity taskEntity)
    {
        if (Tasks.Count >= 20)
        {
            return Error.Validation(description: "Project already has 20 task.");
        }
        
        Tasks.Add(taskEntity);

        return new Success();
    }
}