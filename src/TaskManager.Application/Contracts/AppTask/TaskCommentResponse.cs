using TaskManager.Application.Contracts.Common;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Contracts.AppTask;

public class TaskCommentResponse : AuditableResponse
{
    public string Content { get; set; } = null!;
    public int TaskId { get; set; }
    
    public static TaskCommentResponse FromEntity(TaskCommentEntity entity)
    {
        var response = new TaskCommentResponse
        {
            Content = entity.Content,
            TaskId = entity.TaskId
        };
        
        response.FillFromEntity(entity);

        return response;
    }
}