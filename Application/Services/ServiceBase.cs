using Application.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Application.Interfaces;

namespace Application.Services;

public class ServiceBase(IServiceProvider serviceProvider) : IServiceBase
{
    public int GetAuthenticatedUserId()
    {
        var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();

        if (httpContextAccessor.HttpContext is null)
            throw new NullReferenceException("HttpContext is not set on accessor.");

        return httpContextAccessor.HttpContext.GetUserId();
    }
}