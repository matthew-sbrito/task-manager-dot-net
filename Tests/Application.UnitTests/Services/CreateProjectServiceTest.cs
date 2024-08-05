using Microsoft.AspNetCore.Http;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Validators;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Tests.Common.Factory;

namespace Tests.Application.UnitTests.Services;

public class CreateProjectServiceTest : ServiceBaseTest
{
    private readonly IProjectService _projectService;

    public CreateProjectServiceTest()
    {
        var projectRepositoryMapper = new Mock<IProjectRepository>();
        var projectValidator = new ProjectRequestValidator();

        MockUnitOfWork
            .Setup(x => x.ProjectRepository)
            .Returns(projectRepositoryMapper.Object);

        _projectService = new CreateProjectService(
            MockMapper.Object,
            MockUnitOfWork.Object,
            projectValidator,
            MockServiceProvider.Object
        );
    }

    [Fact]
    public async Task IsValid_ShouldRegisterNew_WhenFieldsGreaterThan5()
    {
        // Arrange
        var request = ProjectFactory.CreateValidPayload();
        var entity = ProjectFactory.ToEntity(request);
        var response = ProjectFactory.ToResponse(entity);

        MockMapper
            .Setup(x => x.Map<ProjectEntity>(request))
            .Returns(entity);
        
        MockMapper
            .Setup(x => x.Map<ProjectResponseDto>(entity))
            .Returns(response);
            
        // Act
        var result = await _projectService
            .CreateProjectAsync(request);

        // Assert
        Assert.True(result.Succeeded);
        Assert.NotNull(result.Data);
        Assert.Null(result.Message);
        Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
    }

    [Fact]
    public async Task IsInvalid_ShouldNotRegisterNew_WhenFieldsLessThan5Chars()
    {
        // Arrange
        var request = ProjectFactory.CreateInvalidPayload();
        const string expectedMessage = "The field title must have minimum 5 char.\nThe field description must have minimum 5 char.";

        // Act
        var result = await _projectService
            .CreateProjectAsync(request);
        
        // Assert
        Assert.False(result.Succeeded);
        Assert.Null(result.Data);
        Assert.Equal(expectedMessage, result.Message);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
    }
}