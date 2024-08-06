using System.Text;
using TaskManager.Application.DTOs.Request;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.ORM;
using TaskManager.Shared.Extensions;
using TaskManager.Shared.Helpers;

namespace TaskManager.Application.Services;

public class TaskHistoryService(
    IUnitOfWork unitOfWork,
    IServiceProvider serviceProvider
) : ServiceBase(serviceProvider), ITaskHistoryService
{
    public async Task RegisterHistory(UserEntity user, TaskEntity task, UpdateTaskDto body)
    {
        var now = DateTimeHelper.UtcNow().ToDefaultFormat();
        var details = new StringBuilder($"User '{user.Name}' updated this task at {now}. \n");
        var taskHistory = new TaskHistoryEntity
        {
            TaskId = task.Id,
            CreatedByUserId = user.Id
        };

        if (!string.Equals(task.Title, body.Title))
            details.Append($"Title was changed from {task.Title} to {body.Title}. \n");

        if (!string.Equals(task.Description, body.Description))
            details.Append($"Description was changed from {task.Description} to {body.Description}. \n");

        if (task.Status != body.Status)
            details.Append($"Status was changed from {task.Status.ToString()} to {body.Status.ToString()}. \n");

        if (task.DueDate != body.DueDate)
            details.Append(
                $"Due date was changed from {task.DueDate.ToDefaultFormat()} to {body.DueDate.ToDefaultFormat()}. \n");

        await unitOfWork.TaskHistoryRepository.AddAsync(taskHistory);
        await unitOfWork.TaskHistoryRepository.SaveAsync();
    }

    public async Task RegisterHistory(UserEntity user, TaskEntity task, CreateTaskCommentDto body)
    {
        var now = DateTimeHelper.UtcNow().ToDefaultFormat();
        var taskHistory = new TaskHistoryEntity
        {
            Details = $"User \"{user.Name}\" commented this task at {now}. \n Comment: \"{body.Content}\" ",
            TaskId = task.Id,
            CreatedByUserId = user.Id
        };

        await unitOfWork.TaskHistoryRepository.AddAsync(taskHistory);
        await unitOfWork.TaskHistoryRepository.SaveAsync();
    }
}