using TaskManager.Application.Contracts.Common;
using TaskManager.Domain.Entities;
using TaskManager.Shared.Enums;

namespace TaskManager.Application.Contracts.AppTask;

public class TaskResponse : AuditableResponse
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public TaskEntityStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime DueDate { get; set; }
    public int ProjectId { get; set; }
    public ICollection<TaskCommentEntity> Comments = [];
    
    public static TaskResponse FromEntity(TaskEntity entity)
    {
        var response = new TaskResponse
        {
            Title = entity.Title,
            Description = entity.Description
        };
        
        response.FillFromEntity(entity);

        return response;
    }
}