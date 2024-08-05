using Domain.Entities;

namespace Application.Interfaces;

public interface IServiceBase
{
    int GetAuthenticatedUserId();
    Task<UserEntity> GetAuthenticatedUser();
}