using Application.DTOs.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;

namespace Application.Interfaces;

public interface ITaskService : IServiceBase
{
    Task<Response<IEnumerable<TaskResponseDto>>> GetTasksByProjectIdAsync(int projectId);
    Task<Response<TaskResponseDto>> CreateTaskAsync(int projectId, CreateTaskDto body);
    Task<Response<TaskResponseDto>> UpdateTaskAsync(int taskId, UpdateTaskDto body);
    Task<Response<TaskCommentResponseDto>> CreateCommentAsync(int taskId, CreateTaskCommentDto body);
    Task<Response<bool>> RemoveTaskAsync(int taskId);
}