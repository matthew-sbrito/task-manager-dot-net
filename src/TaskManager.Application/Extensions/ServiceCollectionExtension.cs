using AutoMapper;
using TaskManager.Domain.ORM;
using FluentValidation;
using FluentValidation.AspNetCore;
using TaskManager.Infrastructure;
using TaskManager.Infrastructure.ORM;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TaskManager.Application.AutoMapper;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Services;

namespace TaskManager.Application.Extensions;

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

    /// <summary>
    /// Add database to app container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
    /// <param name="connectionString">The connection string of current database.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<TaskManagerDbContext>(options => { options.UseNpgsql(connectionString); });

        return services;
    }

    /// <summary>
    /// Add validators to app container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssembly(typeof(IServiceBase).Assembly);

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
}