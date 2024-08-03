using Application.DTOs.Request;
using Domain.Entities;

namespace Application.Interfaces;

public interface ITaskHistoryService
{
    Task RegisterHistory(UserEntity user, TaskEntity task, UpdateTaskDto body);
    Task RegisterHistory(UserEntity user, TaskEntity task, CreateTaskCommentDto body);
}