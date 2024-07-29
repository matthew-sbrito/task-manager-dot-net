using Microsoft.Extensions.DependencyInjection.Extensions;
using Api.Endpoints.Common;
using Api.Filters;
using Common.Enums;

namespace Api.Extensions;

public static class EndpointExtension
{
    /// <summary>
    /// Map endpoint registered as IEndpoint on app container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        var assembly = typeof(IEndpoint).Assembly;
        
        var endpointServiceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(endpointServiceDescriptors);

        return services;
    }

    /// <summary>
    /// Map endpoint registered as IEndpoint on app container.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> instance.</param>
    /// <param name="routeGroupBuilder">The group of endpoints (prefix or anything else).</param>
    /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
    public static IApplicationBuilder MapEndpoints(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return app;
    }

    /// <summary>
    /// Requires that the caller have access.
    /// </summary>
    /// <param name="builder">The endpoint convention builder.</param>
    /// <returns>The original convention builder parameter.</returns>
    public static TBuilder RequireTaskManagerAuthorization<TBuilder>(this TBuilder builder)
        where TBuilder : IEndpointConventionBuilder
    {
        builder.AddEndpointFilter(new TaskManagerAuthorizeFilter());
        return builder;
    }
    
    /// <summary>
    /// Requires that the caller have access to the specified roles.
    /// </summary>
    /// <param name="builder">The endpoint convention builder.</param>
    /// <param name="roles">The required roles for the endpoint.</param>
    /// <returns>The original convention builder parameter.</returns>
    public static TBuilder RequireRoles<TBuilder>(this TBuilder builder, params UserRole[] roles)
        where TBuilder : IEndpointConventionBuilder
    {
        builder.AddEndpointFilter(new TaskManagerAuthorizeFilter(roles));
        return builder;
    }
}