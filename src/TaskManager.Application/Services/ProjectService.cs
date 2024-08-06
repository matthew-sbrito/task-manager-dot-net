using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using TaskManager.Application.DTOs.Common;
using TaskManager.Application.DTOs.Request;
using TaskManager.Application.DTOs.Response;
using TaskManager.Application.Extensions;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.ORM;
using TaskManager.Shared.Enums;

namespace TaskManager.Application.Services;

public class ProjectService(
    IMapper mapper,
    IUnitOfWork unitOfWork,
    IValidator<ProjectRequestDto> createProjectValidator,
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

    public async Task<Response<ProjectResponseDto>> CreateProjectAsync(ProjectRequestDto body)
    {
        var userId = GetAuthenticatedUserId();

        var validationResult = await createProjectValidator.ValidateAsync(body);

        if (!validationResult.IsValid)
            return ResponseService.Error<ProjectResponseDto>(validationResult.GetErrorsMessage());

        var projectEntity = mapper.Map<ProjectEntity>(body);
        projectEntity.CreatedByUserId = userId;

        await unitOfWork.ProjectRepository.AddAsync(projectEntity);
        await unitOfWork.ProjectRepository.SaveAsync();

        var response = mapper.Map<ProjectResponseDto>(projectEntity);
        return ResponseService.Success(response, StatusCodes.Status201Created);
    }

    public async Task<Response<ProjectResponseDto>> UpdateProjectAsync(int projectId, ProjectRequestDto body)
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
            return ResponseService.Error<bool>("Project cannot be removed because has pending task.",
                StatusCodes.Status404NotFound);

        await unitOfWork.ProjectRepository.DeleteAsync(projectEntity);
        await unitOfWork.ProjectRepository.SaveAsync();

        return ResponseService.Success(true, StatusCodes.Status204NoContent);
    }
}