using ErrorOr;
using TaskManager.Application.Contracts.AppTask;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Common.Interfaces;

public interface ITaskHistoryService
{
    Task<ErrorOr<Success>> RegisterHistory(UserEntity user, TaskEntity task, UpdateTaskRequest request);
    Task<ErrorOr<Success>> RegisterHistory(UserEntity user, TaskEntity task, CreateTaskCommentRequest request);
}