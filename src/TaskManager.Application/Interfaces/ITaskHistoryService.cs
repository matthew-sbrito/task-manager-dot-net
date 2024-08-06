using TaskManager.Application.DTOs.Request;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces;

public interface ITaskHistoryService
{
    Task RegisterHistory(UserEntity user, TaskEntity task, UpdateTaskDto body);
    Task RegisterHistory(UserEntity user, TaskEntity task, CreateTaskCommentDto body);
}