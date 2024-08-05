using Tests.Api.IntegrationTest.Common.WebApplicationFactory;
using Xunit.Abstractions;

namespace Tests.Api.IntegrationTest.Common;

[Collection(nameof(IntegrationTestFactoryCollection))]
public abstract class IntegrationBaseTest 
{
    protected readonly HttpClient HttpClient;

    protected IntegrationBaseTest(
        IntegrationTestFactory factory,
        ITestOutputHelper testOutputHelper
    )
    {
        factory.WithTestLogging(testOutputHelper);
        factory.Respawn();
        
        HttpClient = factory.CreateClient();
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