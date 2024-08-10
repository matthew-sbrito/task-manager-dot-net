using System.Text;
using ErrorOr;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Application.Contracts.AppTask;
using TaskManager.Domain.Entities;
using TaskManager.Domain.ORM;
using TaskManager.Shared.Extensions;
using TaskManager.Shared.Helpers;

namespace TaskManager.Application.Services;

public class TaskHistoryService(IUnitOfWork unitOfWork): ITaskHistoryService
{
    public async Task<ErrorOr<Success>> RegisterHistory(UserEntity user, TaskEntity task, UpdateTaskRequest request)
    {
        var now = DateTimeHelper.UtcNow().ToDefaultFormat();
        var details = new StringBuilder($"User '{user.Name}' updated this task at {now}. \n");
        var taskHistory = new TaskHistoryEntity
        {
            TaskId = task.Id,
            CreatedByUserId = user.Id
        };

        if (!string.Equals(task.Title, request.Title))
            details.Append($"Title was changed from {task.Title} to {request.Title}. \n");

        if (!string.Equals(task.Description, request.Description))
            details.Append($"Description was changed from {task.Description} to {request.Description}. \n");

        if (task.Status != request.Status)
            details.Append($"Status was changed from {task.Status.ToString()} to {request.Status.ToString()}. \n");

        if (task.DueDate != request.DueDate)
            details.Append(
                $"Due date was changed from {task.DueDate.ToDefaultFormat()} to {request.DueDate.ToDefaultFormat()}. \n");

        await unitOfWork.TaskHistoryRepository.AddAsync(taskHistory);
        await unitOfWork.SaveAsync();

        return new Success();
    }

    public async Task<ErrorOr<Success>> RegisterHistory(UserEntity user, TaskEntity task, CreateTaskCommentRequest request)
    {
        var now = DateTimeHelper.UtcNow().ToDefaultFormat();
        var taskHistory = new TaskHistoryEntity
        {
            Details = $"User \"{user.Name}\" commented this task at {now}. \n Comment: \"{request.Content}\" ",
            TaskId = task.Id,
            CreatedByUserId = user.Id
        };

        await unitOfWork.TaskHistoryRepository.AddAsync(taskHistory);
        await unitOfWork.SaveAsync();

        return new Success();
    }
}