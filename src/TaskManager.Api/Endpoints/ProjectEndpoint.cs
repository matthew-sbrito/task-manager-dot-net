using TaskManager.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Endpoints.Common;
using TaskManager.Application.DTOs.Request;
using TaskManager.Application.DTOs.Response;
using TaskManager.Application.Interfaces;

namespace TaskManager.Api.Endpoints;

public class ProjectEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var projectGroup = app.MapGroup("/projects")
            .WithTags(EndpointConstants.Tags.Project)
            .RequireTaskManagerAuthorization();

        projectGroup.MapGet("", GetProjectsAsync)
            .Produces<IEnumerable<ProjectResponseDto>>()
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);

        projectGroup.MapPost("", CreateProjectAsync)
            .Produces<ProjectResponseDto>(StatusCodes.Status201Created)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);
        
        var taskWithinProjectGroup = app.MapGroup("/projects/{projectId:int}/tasks")
            .WithTags(EndpointConstants.Tags.Task)
            .RequireTaskManagerAuthorization();

        taskWithinProjectGroup.MapGet("", GetTasksAsync)
            .Produces<IEnumerable<TaskResponseDto>>()
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);

        taskWithinProjectGroup.MapPost("", CreateTaskAsync)
            .Produces<TaskResponseDto>(StatusCodes.Status201Created)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> GetProjectsAsync(
        [FromServices] IProjectService projectService
    )
    {
        var response = await projectService.GetProjectsAsync();
        return response.ToHttpResponse();
    }

    private static async Task<IResult> CreateProjectAsync(
        [FromServices] IProjectService projectService,
        [FromBody] ProjectRequestDto requestBody
    )
    {
        var response = await projectService.CreateProjectAsync(requestBody);

        return response
            .ToCreatedResponse(result => $"/projects/{result.Id}");
    }
    
    private static async Task<IResult> GetTasksAsync(
        [FromRoute] int projectId,
        [FromServices] ITaskService taskService
    )
    {
        var response = await taskService
            .GetTasksByProjectIdAsync(projectId);

        return response.ToHttpResponse();
    }

    private static async Task<IResult> CreateTaskAsync(
        [FromRoute] int projectId,
        [FromBody] CreateTaskDto requestBody,
        [FromServices] ITaskService taskService
    )
    {
        var response = await taskService
            .CreateTaskAsync(projectId, requestBody);

        return response
            .ToCreatedResponse(result => $"/projects/{result.ProjectId}/{result.Id}");
    }
}