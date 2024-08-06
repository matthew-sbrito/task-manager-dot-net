using TaskManager.Application.DTOs.Common;
using TaskManager.Application.DTOs.Request;
using TaskManager.Application.DTOs.Response;

namespace TaskManager.Application.Interfaces;

public interface IProjectService : IServiceBase
{
    Task<Response<IEnumerable<ProjectResponseDto>>> GetProjectsAsync();
    Task<Response<ProjectResponseDto>> CreateProjectAsync(ProjectRequestDto body);
    Task<Response<ProjectResponseDto>> UpdateProjectAsync(int projectId, ProjectRequestDto body);
    Task<Response<bool>> DeleteProjectAsync(int projectId);
}