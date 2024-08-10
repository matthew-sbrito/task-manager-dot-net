using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskManager.Application.Common.Interfaces;

namespace TaskManager.Application.Common.Extensions;

public static class HostExtension
{
    public static async Task RunInitializers(this IHost app)
    {
        await using var scope = app.Services.CreateAsyncScope();

        var appInitializers = scope.ServiceProvider.GetRequiredService<IEnumerable<IAppInitializer>>();
        
        foreach (var initializer in appInitializers)
            await initializer.InitializeAsync();
    }
}