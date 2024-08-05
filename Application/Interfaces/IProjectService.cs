using Application.DTOs.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;

namespace Application.Interfaces;

public interface IProjectService : IServiceBase
{
    Task<Response<IEnumerable<ProjectResponseDto>>> GetProjectsAsync();
    Task<Response<ProjectResponseDto>> CreateProjectAsync(ProjectRequestDto body);
    Task<Response<ProjectResponseDto>> UpdateProjectAsync(int projectId, ProjectRequestDto body);
    Task<Response<bool>> DeleteProjectAsync(int projectId);
}