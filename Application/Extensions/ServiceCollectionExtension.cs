using DbUp;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<TaskManagerDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        return services;
    }
    
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        // services
        //     .AddScoped<>();

        return services;
    }
    
    public static IServiceCollection AddExecuteDbUpMigrations(this IServiceCollection services, string connectionString)
    {
        var scriptPath = Path.GetFullPath(@"..\Infrastructure\DatabaseScripts");
        
        var upgradeEngine = DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScriptsFromFileSystem(scriptPath)
            .JournalToPostgresqlTable("public", "schemaversions")
            .LogToConsole()
            .Build();

        var result = upgradeEngine.PerformUpgrade();

        if (!result.Successful)
            throw new ApplicationException("Database migration failed", result.Error);

        return services;
    }
}