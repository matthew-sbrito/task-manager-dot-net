using Application.DTOs.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.ORM;

namespace Application.Services;

public class ProjectService(
    IMapper mapper,
    IUnitOfWork unitOfWork,
    IServiceProvider serviceProvider
) : ServiceBase(serviceProvider), IProjectService
{
    public async Task<Response<IEnumerable<ProjectResponseDto>>> GetProjectsAsync()
    {
        var userId = GetAuthenticatedUserId();
        var projects = await unitOfWork
            .ProjectRepository.GetProjectByUserIdAsync(userId);
        
        var response = mapper.Map<IEnumerable<ProjectResponseDto>>(projects);
        
        return ResponseService.Success(response);
    }

    public async Task<Response<ProjectResponseDto>> CreateProject(CreateProjectDto body)
    {
        var project = new ProjectResponseDto();

        return ResponseService.Success(project);
    }

    public Task<Response<ProjectResponseDto>> UpdateProject(int projectId, UpdateProjectDto body)
    {
        throw new NotImplementedException();
    }
}