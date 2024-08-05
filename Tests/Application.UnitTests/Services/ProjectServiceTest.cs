using Microsoft.AspNetCore.Http;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Validators;
using Application.Services;
using Common.Helpers;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Tests.Application.UnitTests.Services;

public class ProjectServiceTest : ServiceBaseTest
{
    private readonly IProjectService _projectService;

    public ProjectServiceTest()
    {
        var projectRepositoryMapper = new Mock<IProjectRepository>();
        var projectValidator = new ProjectRequestValidator();

        UnitOfWorkMock
            .Setup(x => x.ProjectRepository)
            .Returns(projectRepositoryMapper.Object);

        _projectService = new ProjectService(
            MapperMock.Object,
            UnitOfWorkMock.Object,
            projectValidator,
            ServiceProviderMock.Object
        );
    }

    [Fact]
    public async Task IsValid_ShouldRegisterNew_WhenFieldsGreaterThan5()
    {
        var request = new ProjectRequestDto
        {
            Title = "Greater than 5",
            Description = "Greater than 5"
        };
        
        var entity = new ProjectEntity
        {
            Id = 1,
            Title = "Greater than 5",
            Description = "Greater than 5",
            CreatedAt = DateTimeHelper.UtcNow(),
            CreatedByUserId = 1
        };
        
        var response = new ProjectResponseDto
        {
            Id = 1,
            Title = "Greater than 5",
            Description = "Greater than 5",
            CreatedAt = DateTimeHelper.UtcNow(),
            CreatedByUserId = 1
        };

        MapperMock
            .Setup(x => x.Map<ProjectEntity>(request))
            .Returns(entity);
        
        MapperMock
            .Setup(x => x.Map<ProjectResponseDto>(entity))
            .Returns(response);
            
        var result = await _projectService
            .CreateProjectAsync(request);

        Assert.True(result.Succeeded);
        Assert.NotNull(result.Data);
        Assert.Null(result.Message);
        Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
    }

    [Fact]
    public async Task IsInvalid_ShouldNotRegisterNew_WhenFieldsLessThan5Chars()
    {
        var request = new ProjectRequestDto
        {
            Title = "Less",
            Description = "Less"
        };
        
        var result = await _projectService
            .CreateProjectAsync(request);

        const string expectedMessage = "The field title must have minimum 5 char.\nThe field description must have minimum 5 char.";

        Assert.False(result.Succeeded);
        Assert.Null(result.Data);
        Assert.Equal(expectedMessage, result.Message);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
    }
}