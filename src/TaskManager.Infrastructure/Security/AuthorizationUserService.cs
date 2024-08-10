using ErrorOr;
using Microsoft.AspNetCore.Http;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Shared.Enums;

namespace TaskManager.Infrastructure.Security;

public class AuthorizationUserService : IAuthorizationUserService
{
    public ErrorOr<Success> UserCanAccess(UserEntity user, List<UserRole> roles)
    {
        if (!roles.Contains(user.Role))
        {
            return Error.Custom(
                type: StatusCodes.Status403Forbidden,
                code: "Forbidden",
                description: "User is missing required roles for taking this action."
            );
        }

        return Result.Success;
    }
}