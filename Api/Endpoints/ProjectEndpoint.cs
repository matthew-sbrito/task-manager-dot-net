using Api.Endpoints.Common;
using Api.Extensions;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public class ProjectEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/projects")
            .WithTags(EndpointConstants.Tags.Project)
            .RequireTaskManagerAuthorization();

        group.MapGet("", GetProjectsAsync)
            .Produces<IEnumerable<ProjectResponseDto>>()
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);

        group.MapPost("", CreateProjectAsync)
            .Produces<ProjectResponseDto>(StatusCodes.Status201Created)
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
        [FromBody] CreateProjectDto requestBody
    )
    {
        var response = await projectService.CreateProjectAsync(requestBody);

        return response
            .ToCreatedResponse(result => $"/projects/{result.Id}");
    }
}