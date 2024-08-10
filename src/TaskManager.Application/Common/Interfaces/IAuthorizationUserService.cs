using ErrorOr;
using TaskManager.Domain.Entities;
using TaskManager.Shared.Enums;

namespace TaskManager.Application.Common.Interfaces;

public interface IAuthorizationUserService
{
    ErrorOr<Success> UserCanAccess(UserEntity user, List<UserRole> roles);
}