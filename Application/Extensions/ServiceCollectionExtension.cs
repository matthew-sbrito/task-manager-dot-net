using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Application.AutoMapper;
using Application.Interfaces;
using Application.Services;
using Infrastructure.ORM;
using Infrastructure;
using AutoMapper;
using Domain.ORM;
using DbUp;

namespace Application.Extensions;

public static class ServiceCollectionExtension
{
    /// <summary>
    /// Map DTOs and Entities in Mapper to using as service
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection MapAutoMapper(this IServiceCollection services)
    {
        return services.AddSingleton(CreateMapper());
    }

    private static IMapper CreateMapper()
    {
        var mapperConfiguration = new MapperConfiguration(configure =>
        {
            configure.AddProfile(new ProjectProfile());
            configure.AddProfile(new TaskProfile());
            configure.AddProfile(new TaskCommentProfile());
        });

        return mapperConfiguration.CreateMapper();
    }
    
    /// <summary>
    /// Execute migration using DbUp library.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
    /// <param name="connectionString">The connection string of current database.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection ExecuteDbUpMigrations(this IServiceCollection services, string connectionString)
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
    
    /// <summary>
    /// Add database to app container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
    /// <param name="connectionString">The connection string of current database.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<TaskManagerDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        return services;
    }
    
    /// <summary>
    /// Add services to app container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddTransient<IUnitOfWork, UnitOfWork>()
            .AddScoped<IProjectService, ProjectService>()
            .AddScoped<ITaskService, TaskService>()
            .AddScoped<ITaskHistoryService, TaskHistoryService>();
        
        return services;
    }
    
    /// <summary>
    /// Add initializers to app container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddInitializers(this IServiceCollection services)
    {
        var assembly = typeof(IAppInitializer).Assembly;
        
        var endpointServiceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IAppInitializer)))
            .Select(type => ServiceDescriptor.Transient(typeof(IAppInitializer), type))
            .ToArray();

        services.TryAddEnumerable(endpointServiceDescriptors);

        return services;
    }
}