using ErrorOr;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Common.Interfaces;

public interface IAuthenticatedUserProvider
{
    Task<ErrorOr<UserEntity>> GetCurrentUser();
}