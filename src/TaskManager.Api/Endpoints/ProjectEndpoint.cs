using MediatR;
using TaskManager.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Endpoints.Common;
using TaskManager.Application.AppTask.Commands.CreateTask;
using TaskManager.Application.AppTask.Queries.ListTask;
using TaskManager.Application.Contracts.AppTask;
using TaskManager.Application.Contracts.Projects;
using TaskManager.Application.Projects.Commands.CreateProject;
using TaskManager.Application.Projects.Commands.UpdateProject;
using TaskManager.Application.Projects.Queries.ListProject;
using TaskManager.Infrastructure.Extensions;

namespace TaskManager.Api.Endpoints;

public class ProjectEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var projectGroup = app.MapGroup("/projects")
            .WithTags(EndpointConstants.Tags.Project)
            .RequireTaskManagerAuthorization();

        projectGroup.MapGet("", GetProjectsAsync)
            .Produces<IEnumerable<ProjectResponse>>()
            .Produces(StatusCodes.Status400BadRequest);

        projectGroup.MapPost("", CreateProjectAsync)
            .Produces<ProjectResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        projectGroup.MapPut("", UpdateProjectAsync)
            .Produces<ProjectResponse>()
            .Produces(StatusCodes.Status400BadRequest);

        var taskWithinProjectGroup = app.MapGroup("/projects/{projectId:int}/tasks")
            .WithTags(EndpointConstants.Tags.Task)
            .RequireTaskManagerAuthorization();

        taskWithinProjectGroup.MapGet("", GetTasksAsync)
            .Produces<IEnumerable<TaskResponse>>()
            .Produces(StatusCodes.Status400BadRequest);

        taskWithinProjectGroup.MapPost("", CreateTaskAsync)
            .Produces<TaskResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> GetProjectsAsync(
        [FromServices] HttpContext context,
        [FromServices] ISender sender
    )
    {
        var userId = context.GetUserId();

        if (userId.IsError)
            return userId.Errors.ToProblemDetails();
        
        var command = new ListProjectQuery(userId.Value);
        var response = await sender.Send(command);
        
        return response
            .Match(Results.Ok, ErrorExtension.ToProblemDetails);
    }

    private static async Task<IResult> CreateProjectAsync(
        [FromServices] HttpContext context,
        [FromServices] ISender sender,
        [FromBody] ProjectRequest request
    )
    {
        var userId = context.GetUserId();

        if (userId.IsError)
            return userId.Errors.ToProblemDetails();

        var command = new CreateProjectCommand(userId.Value, request.Title, request.Description);
        var response = await sender.Send(command);

        return response
            .Match(x => Results.Created($"/projects/{x.Id}", x), ErrorExtension.ToProblemDetails);
    }

    private static async Task<IResult> UpdateProjectAsync(
        [FromServices] HttpContext context,
        [FromServices] ISender sender,
        [FromRoute] int projectId,
        [FromBody] ProjectRequest request
    )
    {
        var userId = context.GetUserId();

        if (userId.IsError)
            return userId.Errors.ToProblemDetails();

        var command = new UpdateProjectCommand(userId.Value, projectId, request.Title, request.Description);
        var response = await sender.Send(command);

        return response
            .Match(Results.Ok, ErrorExtension.ToProblemDetails);
    }

    private static async Task<IResult> GetTasksAsync(
        [FromServices] HttpContext context,
        [FromServices] ISender sender,
        [FromRoute] int projectId
    )
    {
        var userId = context.GetUserId();

        if (userId.IsError)
            return userId.Errors.ToProblemDetails();
        
        var command = new ListTaskQuery(userId.Value, projectId);
        var response = await sender.Send(command);
        
        return response
            .Match(Results.Ok, ErrorExtension.ToProblemDetails);
    }

    private static async Task<IResult> CreateTaskAsync(
        [FromServices] HttpContext context,
        [FromServices] ISender sender,
        [FromRoute] int projectId,
        [FromBody] CreateTaskRequest request
    )
    {
        var userId = context.GetUserId();

        if (userId.IsError)
            return userId.Errors.ToProblemDetails();

        var command = new CreateTaskCommand(userId.Value, projectId, request);
        var response = await sender.Send(command);

        return response
            .Match(x => Results.Created($"/projects/{x.ProjectId}/task/{x.Id}", x), Results.BadRequest);
    }
}