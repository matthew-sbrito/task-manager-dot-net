using ErrorOr;
using MediatR;
using TaskManager.Application.Contracts.AppTask;
using TaskManager.Domain.Entities;
using TaskManager.Domain.ORM;
using TaskManager.Shared.Enums;

namespace TaskManager.Application.AppTask.Commands.CreateTask;

public class CreateTaskCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateTaskCommand, ErrorOr<TaskResponse>>
{
    public async Task<ErrorOr<TaskResponse>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var projectEntity = await unitOfWork.ProjectRepository
            .GetByIdAsync(request.ProjectId, ["Tasks"]);

        if (projectEntity is null)
        {
            return Error.Validation(description: "Project does not exists.");
        }
        
        var taskEntity = new TaskEntity
        {
            Title = request.Body.Title,
            Description = request.Body.Description,
            Priority = request.Body.Priority,
            DueDate = request.Body.DueDate,
            Status = TaskEntityStatus.Pending,
            CreatedByUserId = request.AuthenticatedUserId
        };

        var addTaskResult = projectEntity.AddTask(taskEntity);

        if (addTaskResult.IsError)
        {
            return addTaskResult.Errors;
        }

        await unitOfWork.ProjectRepository.UpdateAsync(projectEntity);
        await unitOfWork.TaskRepository.AddAsync(taskEntity);
        await unitOfWork.SaveAsync();

        return TaskResponse.FromEntity(taskEntity);
    }
}