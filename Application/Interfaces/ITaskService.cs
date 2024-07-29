using Application.DTOs.Request;
using Application.DTOs.Response;

namespace Application.Interfaces;

public interface ITaskService : IServiceBase
{
    Task<IEnumerable<TaskResponseDto>> GetTasksByProjectIdAsync(int projectId);
    Task<TaskResponseDto> CreateTask(int projectId, CreateTaskDto body);
    Task<TaskResponseDto> UpdateTask(int taskId, UpdateTaskDto body);
    Task RemoveTask(int taskId);
}