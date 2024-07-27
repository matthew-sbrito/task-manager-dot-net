using Application.AutoMapper;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class AutoMapperExtension
{
    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        return services.AddSingleton(CreateMapper());
    }

    private static IMapper CreateMapper()
    {
        var mapperConfiguration = new MapperConfiguration(configure =>
        {
            configure.AddProfile(new ProjectProfile());
            configure.AddProfile(new TaskProfile());
        });

        return mapperConfiguration.CreateMapper();
    }
}