using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.ORM;

namespace Application.Services;

public class TaskService(
    IServiceProvider serviceProvider, 
    IUnitOfWork unitOfWork
) : ServiceBase(serviceProvider), ITaskService
{
    public Task<IEnumerable<TaskResponseDto>> GetTasksByProjectIdAsync(int projectId)
    {
        throw new NotImplementedException();
    }

    public Task<TaskResponseDto> CreateTask(int projectId, CreateTaskDto body)
    {
        throw new NotImplementedException();
    }

    public Task<TaskResponseDto> UpdateTask(int taskId, UpdateTaskDto body)
    {
        throw new NotImplementedException();
    }

    public Task RemoveTask(int taskId)
    {
        throw new NotImplementedException();
    }
}