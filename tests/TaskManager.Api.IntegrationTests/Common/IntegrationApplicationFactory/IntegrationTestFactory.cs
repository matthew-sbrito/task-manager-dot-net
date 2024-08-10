using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TaskManager.Application.Common.Extensions;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Infrastructure;
using TaskManager.Infrastructure.Configurations;
using TaskManager.Infrastructure.Extensions;
using Testcontainers.PostgreSql;

namespace TaskManager.Api.IntegrationTests.Common.IntegrationApplicationFactory;

public class IntegrationTestFactory
    : WebApplicationFactory<IAssemblyMarker>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer =
        new PostgreSqlBuilder()
            .WithDatabase("task-manager-db")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var connectionString = _dbContainer.GetConnectionString();

        builder.UseEnvironment("Testing");
        builder.ConfigureAppConfiguration(configure =>
        {
            configure.Properties.Add("DefaultConnection", connectionString);
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<TaskManagerDbContext>>();
            services.RemoveAll<TaskManagerDbContext>();

            DatabaseMigrationHelper.ExecuteMigrations(connectionString);
            services.AddDatabase(connectionString);
        });
    }

    public Task InitializeAsync()
    {
        return _dbContainer.StartAsync();
    }

    public new Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }

    public async Task UpDatabase()
    {
        var connectionString = _dbContainer.GetConnectionString();
        DatabaseMigrationHelper.ExecuteMigrations(connectionString);
        
        using var scope = Services.CreateScope();
        
        var appInitializers = scope.ServiceProvider.GetRequiredService<IEnumerable<IAppInitializer>>();
        
        foreach (var initializer in appInitializers)
            await initializer.InitializeAsync();
    }
    
    public Task DownDatabase()
    {
        var connectionString = _dbContainer.GetConnectionString();
        DatabaseMigrationHelper.CleanDatabase(connectionString);
        
        return Task.CompletedTask;
    }
}