using Meziantou.Extensions.Logging.Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Tests.Api.IntegrationTest.Common.WebApplicationFactory;

internal static class IntegrationTestFactoryExtensions
{
    public static void WithTestLogging<TStartup>(
        this WebApplicationFactory<TStartup> factory,
        ITestOutputHelper testOutputHelper
    ) where TStartup : class
    {
        factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureLogging(x =>
            {
                x.ClearProviders();
                x.Services.AddSingleton<ILoggerProvider>(new XUnitLoggerProvider(testOutputHelper));
            });
        });
    }
}