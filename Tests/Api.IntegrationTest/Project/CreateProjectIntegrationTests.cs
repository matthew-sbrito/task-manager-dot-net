using System.Net;
using System.Net.Http.Json;
using Application.DTOs.Response;
using Microsoft.AspNetCore.Http;
using Tests.Api.IntegrationTest.Common;
using Tests.Api.IntegrationTest.Common.WebApplicationFactory;
using Tests.Common.Factory;
using Xunit.Abstractions;

namespace Tests.Api.IntegrationTest.Project;

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

        var projectResponse = await response.Content.ReadFromJsonAsync<ProjectResponseDto>();

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
        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

        // Assert
        response.Should().HaveError();
        response.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        errorResponse.Should().NotBeNull();
        errorResponse?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        errorResponse?.Message.Should().Be(expectedMessage);
    }
}