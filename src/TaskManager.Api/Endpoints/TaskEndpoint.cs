using MediatR;
using TaskManager.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Endpoints.Common;
using TaskManager.Application.AppTask.Commands.CreateTaskComment;
using TaskManager.Application.AppTask.Commands.DeleteTask;
using TaskManager.Application.AppTask.Commands.UpdateTask;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Application.Contracts.AppTask;
using TaskManager.Infrastructure.Extensions;

namespace TaskManager.Api.Endpoints;

public class TaskEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/tasks/{taskId:int}")
            .WithTags(EndpointConstants.Tags.Task)
            .RequireTaskManagerAuthorization();

        group.MapPut("", UpdateTaskAsync)
            .Produces<TaskResponse>()
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapDelete("", DeleteTaskAsync)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapPost("/comment", CreateCommentTaskAsync)
            .Produces<TaskCommentResponse>()
            .Produces(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> UpdateTaskAsync(
        [FromServices] HttpContext context,
        [FromServices] ISender sender,
        [FromRoute] int taskId,
        [FromBody] UpdateTaskRequest request
    )
    {
        var userId = context.GetUserId();

        if (userId.IsError)
            return userId.Errors.ToProblemDetails();
        
        var command = new UpdateTaskCommand(userId.Value, taskId, request);
        var response = await sender.Send(command);
        
        return response
            .Match(Results.Ok, ErrorExtension.ToProblemDetails);
    }

    private static async Task<IResult> CreateCommentTaskAsync(        
        [FromServices] HttpContext context,
        [FromServices] ISender sender,
        [FromRoute] int taskId,
        [FromBody] CreateTaskCommentRequest request
    )
    {
        var userId = context.GetUserId();

        if (userId.IsError)
            return userId.Errors.ToProblemDetails();
        
        var command = new CreateTaskCommentCommand(userId.Value, taskId, request);
        var response = await sender.Send(command);
        
        return response
            .Match(x => Results.Created($"/tasks/{x.TaskId}", x), ErrorExtension.ToProblemDetails);
    }

    private static async Task<IResult> DeleteTaskAsync(
        [FromServices] HttpContext context,
        [FromServices] ISender sender,
        [FromRoute] int taskId
    )
    {
        var userId = context.GetUserId();

        if (userId.IsError)
            return userId.Errors.ToProblemDetails();
        
        var command = new DeleteTaskCommand(userId.Value, taskId);
        var response = await sender.Send(command);
        
        return response
            .Match(_ => Results.NoContent(), ErrorExtension.ToProblemDetails);
    }
}