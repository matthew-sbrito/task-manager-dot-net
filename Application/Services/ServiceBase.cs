using Application.Exceptions;
using Application.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Application.Interfaces;
using Domain.Entities;
using Domain.ORM;

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
    
    public async Task<UserEntity> GetAuthenticatedUser()
    {
        var userId = GetAuthenticatedUserId();
        
        var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

        if (unitOfWork is null)
            throw new NullReferenceException("HttpContext is not set on accessor.");

        var user = await unitOfWork.UserRepository.GetByIdAsync(userId);
        
        if (user is null)
            throw new UnauthorizedException("User does not exists.");

        return user;
    }
}