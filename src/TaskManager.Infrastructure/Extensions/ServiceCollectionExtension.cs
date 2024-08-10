using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Application.Common.Security.Request;
using TaskManager.Domain.ORM;
using TaskManager.Infrastructure.ORM;

namespace TaskManager.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
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
            .AddFluentValidationClientsideAdapters();
            // .AddValidatorsFromAssembly(typeof().Assembly);

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
            .AddMediatR(configure =>
            {
                configure.RegisterServicesFromAssemblies(typeof(IAuthorizedRequest<>).Assembly);
            });

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