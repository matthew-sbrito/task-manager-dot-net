using ErrorOr;
using TaskManager.Domain.ORM;
using Microsoft.AspNetCore.Http;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Extensions;
using TaskManager.Application.Common.Interfaces;

namespace TaskManager.Infrastructure.Security;

public class AuthenticatedUserProvider(
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor
) : IAuthenticatedUserProvider
{
    public async Task<ErrorOr<UserEntity>> GetCurrentUser()
    {
        if (httpContextAccessor.HttpContext == null)
            throw new NullReferenceException("HttpContext is null on IHttpContextAccessor.");

        var userIdValidated = httpContextAccessor.HttpContext.GetUserId();

        if (userIdValidated.IsError)
            return userIdValidated.Errors;

        var user = await unitOfWork.UserRepository.GetByIdAsync(userIdValidated.Value);

        if (user is null)
            return Error.Unauthorized(description: "User does not exists.");

        return user;
    }
}