using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using TaskManager.Api.IntegrationTests.Common;
using TaskManager.Api.IntegrationTests.Common.IntegrationApplicationFactory;
using TaskManager.Application.Contracts.Common;
using TaskManager.Application.Contracts.Projects;
using TaskManager.TestCommon.Project;
using Xunit.Abstractions;

namespace TaskManager.Api.IntegrationTests.Project;

public class CreateProjectIntegrationTests(
    IntegrationTestFactory factory,
    ITestOutputHelper testOutputHelper
) : IntegrationBaseTest(factory, testOutputHelper)
{
    [Fact]
    public async Task CreateProjectRequest_WhenPayloadIsValid_ShouldCreateProject()
    {
        // Arrange
        SetManagerOnHeader();
        var projectRequest = ProjectFactory.CreateValidPayload();

        // Act
        var response = await HttpClient
            .PostAsJsonAsync("/api/v1/projects", projectRequest);

        var projectResponse = await response.Content.ReadFromJsonAsync<ProjectResponse>();

        // Assert
        response.Should().BeSuccessful();
        response.Should().HaveStatusCode(HttpStatusCode.Created);
        projectResponse.Should().NotBeNull();
        projectResponse?.Id.Should().Be(1);
        projectResponse?.Title.Should().Be(projectRequest.Title);
        projectResponse?.Description.Should().Be(projectRequest.Description);
    }

    [Fact]
    public async Task CreateProjectRequest_WhenPayloadIsInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        SetManagerOnHeader();
        var projectRequest = ProjectFactory.CreateInvalidPayload();
        const string expectedMessage =
            "The field title must have minimum 5 char.\nThe field description must have minimum 5 char.";

        // Act
        var response = await HttpClient
            .PostAsJsonAsync("/api/v1/projects", projectRequest);

        // Assert
        response.Should().HaveError();
        response.Should().HaveStatusCode(HttpStatusCode.BadRequest);
    }
}