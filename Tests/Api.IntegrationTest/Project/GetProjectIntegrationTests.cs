using System.Net;
using System.Net.Http.Json;
using Application.DTOs.Response;
using Tests.Api.IntegrationTest.Common;
using Tests.Api.IntegrationTest.Common.WebApplicationFactory;
using Tests.Common.Factory;
using Xunit.Abstractions;

namespace Tests.Api.IntegrationTest.Project;

public class GetProjectIntegrationTests(
    IntegrationTestFactory factory,
    ITestOutputHelper testOutputHelper
) : IntegrationBaseTest(factory, testOutputHelper)
{
    [Fact]
    public async Task NoSetProjects_WhenTryGetList_ShouldReturnEmpty()
    {
        // Arrange
        SetManagerOnHeader();
        
        // Act
        var response = await HttpClient.GetAsync("/api/v1/projects");
        var responseBody = await response.Content.ReadFromJsonAsync<ICollection<ProjectResponseDto>>();

        // Assert
        response.Should().BeSuccessful();
        response.Should().HaveStatusCode(HttpStatusCode.OK);
        responseBody.Should().NotBeNull();
        responseBody.Should().BeEmpty();
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task SetProjects_WhenTryGetList_ShouldReturnProjects(int count)
    {
        // Arrange
        SetManagerOnHeader();
        var requestBodyList = ProjectFactory.CreateValidPayload(count);

        // Act
        foreach (var requestBody in requestBodyList)
        {
            await HttpClient
                .PostAsJsonAsync("/api/v1/projects", requestBody);
        }

        var response = await HttpClient.GetAsync("/api/v1/projects");
        var responseBody = await response.Content.ReadFromJsonAsync<ICollection<ProjectResponseDto>>();

        // Assert
        response.Should().BeSuccessful();
        response.Should().HaveStatusCode(HttpStatusCode.OK);
        responseBody.Should().NotBeNull();
        responseBody.Should().HaveCount(count);
    }
}