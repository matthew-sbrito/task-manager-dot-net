using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Exceptions;
using TaskManager.Application.Extensions;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.ORM;

namespace TaskManager.Application.Services;

public class ServiceBase(IServiceProvider serviceProvider) : IServiceBase
{
    public int GetAuthenticatedUserId()
    {
        var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();

        if (httpContextAccessor.HttpContext is null)
        {
            throw new NullReferenceException("HttpContext is not set on accessor.");
        }

        return httpContextAccessor.HttpContext.GetUserId();
    }

    public async Task<UserEntity> GetAuthenticatedUser()
    {
        var userId = GetAuthenticatedUserId();

        var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

        if (unitOfWork is null)
        {
            throw new NullReferenceException("HttpContext is not set on accessor.");
        }

        var user = await unitOfWork.UserRepository.GetByIdAsync(userId);

        if (user is null)
        {
            throw new UnauthorizedException("User does not exists.");
        }

        return user;
    }
}