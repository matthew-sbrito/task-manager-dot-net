using System.Net;
using System.Net.Http.Json;
using TaskManager.Api.IntegrationTests.Common;
using TaskManager.Api.IntegrationTests.Common.IntegrationApplicationFactory;
using TaskManager.Application.Contracts.Common;
using TaskManager.Application.Contracts.Projects;
using Xunit.Abstractions;

namespace TaskManager.Api.IntegrationTests.Authentication;

public class AuthenticationIntegrationTests(
    IntegrationTestFactory factory,
    ITestOutputHelper testOutputHelper
) : IntegrationBaseTest(factory, testOutputHelper)
{
    [Fact]
    public async Task SetUserOnHeader_WhenUserIsValid_ShouldReturnData()
    {
        // Arrange
        SetManagerOnHeader();

        // Act
        var response = await HttpClient.GetAsync("/api/v1/projects");
        var responseBody = await response.Content.ReadFromJsonAsync<IEnumerable<ProjectResponse>>();

        // Assert
        response.Should().BeSuccessful();
        response.Should().HaveStatusCode(HttpStatusCode.OK);
        responseBody.Should().NotBeNull();
    }
    
    [Fact]
    public async Task NotSetUserOnHeader_WhenUserIsInvalid_ShouldReturnUnauthorized()
    {
        // Arrange
        // Do nothing

        // Act
        var response = await HttpClient.GetAsync("/api/v1/projects");

        // Assert
        response.Should().HaveError();
        response.Should().HaveStatusCode(HttpStatusCode.Unauthorized);
    }
}