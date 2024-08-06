using TaskManager.Application.DTOs.Common;
using TaskManager.Application.DTOs.Request;
using TaskManager.Application.DTOs.Response;

namespace TaskManager.Application.Interfaces;

public interface ITaskService : IServiceBase
{
    Task<Response<IEnumerable<TaskResponseDto>>> GetTasksByProjectIdAsync(int projectId);
    Task<Response<TaskResponseDto>> CreateTaskAsync(int projectId, CreateTaskDto body);
    Task<Response<TaskResponseDto>> UpdateTaskAsync(int taskId, UpdateTaskDto body);
    Task<Response<TaskCommentResponseDto>> CreateCommentAsync(int taskId, CreateTaskCommentDto body);
    Task<Response<bool>> RemoveTaskAsync(int taskId);
}