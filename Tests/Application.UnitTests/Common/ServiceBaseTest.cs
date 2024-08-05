using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Application.Interfaces;
using Domain.Entities;
using Common.Enums;
using Domain.ORM;
using AutoMapper;
using Moq;

namespace Tests.Application.UnitTests.Services;

public abstract class ServiceBaseTest
{
    protected readonly Mock<IMapper> MockMapper;
    protected readonly Mock<IUnitOfWork> MockUnitOfWork;
    protected readonly Mock<IServiceProvider> MockServiceProvider;

    protected ServiceBaseTest()
    {
        MockMapper = new Mock<IMapper>();
        MockUnitOfWork = new Mock<IUnitOfWork>();
        
        var headers = new HeaderDictionary();
        headers.Append("x-user-id", "1");
        
        var httpContextMock = new Mock<HttpContext>();
        httpContextMock
            .Setup(x => x.Request.Headers)
            .Returns(headers);
        
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock
            .Setup(x => x.HttpContext)
            .Returns(httpContextMock.Object);
        
        MockServiceProvider = new Mock<IServiceProvider>();
        MockServiceProvider
            .Setup(x => x.GetService(typeof(IHttpContextAccessor)))
            .Returns(httpContextAccessorMock.Object);
    }
    
    protected static void SetupManagerUser<TService>(Mock<TService> serviceBase) where TService : class, IServiceBase
    {
        serviceBase
            .Setup(x => x.GetAuthenticatedUserId())
            .Returns(1);

        var userEntity = new UserEntity { Id = 1, Name = "Test", Role = UserRole.Manager };
        
        serviceBase
            .Setup(x => x.GetAuthenticatedUser())
            .Returns(Task.FromResult(userEntity));
    }
    
    protected static void SetupCommonUser<TService>(Mock<TService> serviceBase) where TService : class, IServiceBase
    {
        serviceBase
            .Setup(x => x.GetAuthenticatedUserId())
            .Returns(1);

        var userEntity = new UserEntity { Id = 1, Name = "Test", Role = UserRole.Common };
        
        serviceBase
            .Setup(x => x.GetAuthenticatedUser())
            .Returns(Task.FromResult(userEntity));
    }
}