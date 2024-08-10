using TaskManager.Application.Contracts.AppTask;
using TaskManager.Application.Contracts.Common;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Contracts.Projects;

public class ProjectResponse : AuditableResponse
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ICollection<TaskResponse> Tasks { get; set; } = [];

    public static ProjectResponse FromEntity(ProjectEntity entity)
    {
        var response = new ProjectResponse
        {
            Title = entity.Title,
            Description = entity.Description,
            Tasks = entity.Tasks
                .Select(TaskResponse.FromEntity)
                .ToList()
        };
        
        response.FillFromEntity(entity);

        return response;
    }
}