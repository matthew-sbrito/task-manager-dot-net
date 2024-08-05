using Api.Endpoints.Common;
using Api.Extensions;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

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
        [FromRoute] int projectId,
        [FromRoute] int taskId,
        [FromBody] UpdateTaskDto requestBody,
        [FromServices] ITaskService taskService
    )
    {
        var response = await taskService
            .UpdateTaskAsync(projectId, taskId, requestBody);

        return response.ToHttpResponse();
    }

    private static async Task<IResult> CreateCommentTaskAsync(
        [FromRoute] int projectId,
        [FromRoute] int taskId,
        [FromBody] CreateTaskCommentDto requestBody,
        [FromServices] ITaskService taskService
    )
    {
        var response = await taskService
            .CreateCommentAsync(projectId, taskId, requestBody);

        return response.ToHttpResponse();
    }

    private static async Task<IResult> DeleteTaskAsync(
        [FromRoute] int projectId,
        [FromRoute] int taskId,
        [FromServices] ITaskService taskService
    )
    {
        var response = await taskService
            .RemoveTaskAsync(projectId, taskId);

        return response.ToHttpResponse();
    }
}