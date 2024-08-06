using TaskManager.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Endpoints.Common;
using TaskManager.Application.DTOs.Request;
using TaskManager.Application.DTOs.Response;
using TaskManager.Application.Interfaces;

namespace TaskManager.Api.Endpoints;

public class TaskEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/tasks/{taskId:int}")
            .WithTags(EndpointConstants.Tags.Task)
            .RequireTaskManagerAuthorization();

        group.MapPut("", UpdateTaskAsync)
            .Produces<TaskResponseDto>()
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);
        
        group.MapDelete("", DeleteTaskAsync)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);
        
        group.MapPost("/comment", CreateCommentTaskAsync)
            .Produces<TaskCommentResponseDto>()
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> UpdateTaskAsync(        
        [FromRoute] int taskId,
        [FromBody] UpdateTaskDto requestBody,
        [FromServices] ITaskService taskService
    )
    {
        var response = await taskService
            .UpdateTaskAsync(taskId, requestBody);

        return response.ToHttpResponse();
    }

    private static async Task<IResult> CreateCommentTaskAsync(        
        [FromRoute] int taskId,
        [FromBody] CreateTaskCommentDto requestBody,
        [FromServices] ITaskService taskService
    )
    {
        var response = await taskService
            .CreateCommentAsync(taskId, requestBody);

        return response.ToHttpResponse();
    }

    private static async Task<IResult> DeleteTaskAsync(
        [FromRoute] int taskId,
        [FromServices] ITaskService taskService
    )
    {
        var response = await taskService
            .RemoveTaskAsync(taskId);

        return response.ToHttpResponse();
    }
}