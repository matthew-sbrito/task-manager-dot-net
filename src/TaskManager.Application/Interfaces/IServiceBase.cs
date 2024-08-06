using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces;

public interface IServiceBase
{
    int GetAuthenticatedUserId();
    Task<UserEntity> GetAuthenticatedUser();
}