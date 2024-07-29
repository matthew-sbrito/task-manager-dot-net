using Application.DTOs.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;

namespace Application.Interfaces;

public interface IProjectService : IServiceBase
{
    Task<Response<IEnumerable<ProjectResponseDto>>> GetProjectsAsync();
    Task<Response<ProjectResponseDto>> CreateProject(CreateProjectDto body);
    Task<Response<ProjectResponseDto>> UpdateProject(int projectId, UpdateProjectDto body);
}