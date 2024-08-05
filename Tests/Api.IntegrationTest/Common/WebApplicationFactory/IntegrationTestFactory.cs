using Api;
using Application.Extensions;
using Application.Interfaces;
using Infrastructure;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;

namespace Tests.Api.IntegrationTest.Common.WebApplicationFactory;

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

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<TaskManagerDbContext>>();
            services.RemoveAll<TaskManagerDbContext>();

            DatabaseConfiguration.ExecuteMigrations(connectionString);
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

    public void Respawn()
    {
        var connectionString = _dbContainer.GetConnectionString();
        
        DatabaseConfiguration.CleanDatabase(connectionString);
        DatabaseConfiguration.ExecuteMigrations(connectionString);
        
        using var scope = Services.CreateScope();
        
        var appInitializers = scope.ServiceProvider.GetRequiredService<IEnumerable<IAppInitializer>>();
        
        foreach (var initializer in appInitializers)
            initializer.InitializeAsync().Wait();
    }
}