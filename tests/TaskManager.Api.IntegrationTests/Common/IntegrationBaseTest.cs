using TaskManager.Api.IntegrationTests.Common.IntegrationApplicationFactory;
using Xunit.Abstractions;

namespace TaskManager.Api.IntegrationTests.Common;

[Collection(nameof(IntegrationTestFactoryCollection))]
public abstract class IntegrationBaseTest(
    IntegrationTestFactory factory,
    ITestOutputHelper testOutputHelper
) : IAsyncLifetime
{
    protected HttpClient HttpClient = null!;

    public async Task InitializeAsync()
    {
        factory.WithTestLogging(testOutputHelper);
        await factory.UpDatabase();
        
        HttpClient = factory.CreateClient();
    }

    public async Task DisposeAsync()
    {
        await factory.DownDatabase();
    }

    protected void SetManagerOnHeader()
    {
        SetHeader("x-user-id", "1");
    }

    protected void SetCommonUserOnHeader()
    {
        SetHeader("x-user-id", "2");
    }

    private void SetHeader(string key, string value)
    {
        if (HttpClient.DefaultRequestHeaders.Contains(key))
        {
            HttpClient.DefaultRequestHeaders.Remove(key);
        }

        HttpClient.DefaultRequestHeaders.Add(key, value);
    }
}