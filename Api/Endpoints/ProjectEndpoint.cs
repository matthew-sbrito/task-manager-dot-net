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
        var group = app.MapGroup("/project")
            .WithTags(EndpointConstants.Tags.Project)
            .RequireTaskManagerAuthorization();

        group.MapGet("", GetProjectsAsync)
            .Produces<ProjectResponseDto>(StatusCodes.Status201Created)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);
        
        group.MapPost("", CreateProjectAsync)
            .Produces<ProjectResponseDto>(StatusCodes.Status201Created)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);
    }
    
    private static async Task<IResult> GetProjectsAsync(IProjectService projectService)
    {
        var response = await projectService.GetProjectsAsync();
        return response.ToHttpResponse();
    }

    private static async Task<IResult> CreateProjectAsync(
        IProjectService projectService,
        [FromBody] CreateProjectDto requestBody
    )
    {
        var response = await projectService.CreateProject(requestBody);

        return response
            .ToCreatedResponse(result => $"/project/{result.Id}");
    }
}