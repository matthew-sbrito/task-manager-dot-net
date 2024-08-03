using Application.DTOs.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Common.Enums;
using Domain.Entities;
using Domain.ORM;
using Microsoft.AspNetCore.Http;

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

    public async Task<Response<ProjectResponseDto>> CreateProjectAsync(CreateProjectDto body)
    {
        var userId = GetAuthenticatedUserId();

        var projectEntity = mapper.Map<ProjectEntity>(body);
        projectEntity.CreatedByUserId = userId;

        await unitOfWork.ProjectRepository.AddAsync(projectEntity);
        await unitOfWork.ProjectRepository.SaveAsync();

        var response = mapper.Map<ProjectResponseDto>(projectEntity);
        return ResponseService.Success(response, StatusCodes.Status201Created);
    }

    public async Task<Response<ProjectResponseDto>> UpdateProjectAsync(int projectId, UpdateProjectDto body)
    {
        var projectEntity = await unitOfWork.ProjectRepository.GetByIdAsync(projectId);

        if (projectEntity is null)
            return ResponseService.Error<ProjectResponseDto>("Project not found.", StatusCodes.Status404NotFound);

        projectEntity.Title = body.Title;
        projectEntity.Description = body.Description;
        
        await unitOfWork.ProjectRepository.UpdateAsync(projectEntity);
        await unitOfWork.ProjectRepository.SaveAsync();
        
        var response = mapper.Map<ProjectResponseDto>(projectEntity);
        return ResponseService.Success(response);
    }
    
    public async Task<Response<bool>> DeleteProjectAsync(int projectId)
    {
        var projectEntity = await unitOfWork.ProjectRepository.GetByIdAsync(projectId, ["Tasks"]);

        if (projectEntity is null)
            return ResponseService.Error<bool>("Project not found.", StatusCodes.Status404NotFound);
        
        if (projectEntity.Tasks.All(x => x.Status == TaskEntityStatus.Concluded))
            return ResponseService.Error<bool>("Project cannot be removed because has pending task.", StatusCodes.Status404NotFound);
        
        await unitOfWork.ProjectRepository.DeleteAsync(projectEntity);
        await unitOfWork.ProjectRepository.SaveAsync();

        return ResponseService.Success(true, StatusCodes.Status204NoContent);
    }
}