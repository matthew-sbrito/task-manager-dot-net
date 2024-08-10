using TaskManager.Application.Common.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.ORM;
using TaskManager.Shared.Enums;

namespace TaskManager.Application.Common.Initializers;

public class AuthorizationSeeder(IUnitOfWork unitOfWork) : IAppInitializer
{
    public async Task InitializeAsync()
    {
        var users = await unitOfWork.UserRepository.GetAll();
        
        if (users.Any()) return;
        
        await unitOfWork.UserRepository.AddRangeAsync([
            CreateCommonUser(),
            CreateManagerUser()
        ]);
        
        await unitOfWork.SaveAsync();
    }

    private static UserEntity CreateCommonUser()
    {
        return new UserEntity
        {
            Name = "John Doe",
            Role = UserRole.Common
        };
    }

    private static UserEntity CreateManagerUser()
    {
        return new UserEntity
        {
            Name = "Jane Doe",
            Role = UserRole.Manager
        };
    }
}