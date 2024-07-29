using Application.Exceptions;
using Application.Extensions;
using Common.Enums;
using Domain.ORM;

namespace Api.Filters;

public class TaskManagerAuthorizeFilter : IEndpointFilter
{
    private readonly List<UserRole> _roles;

    public TaskManagerAuthorizeFilter()
    {
        _roles = new List<UserRole>();
    }

    public TaskManagerAuthorizeFilter(params UserRole[] roles)
    {
        _roles = roles.ToList();
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var unitOfWork = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();

        var userId = context.HttpContext.GetUserId();
        var user = await unitOfWork.UserRepository.GetByIdAsync(userId);

        if (user is not { DeletedAt: null })
            throw new UnauthorizedException("User authenticated is not valid.");


        if (_roles.Count > 0 && !_roles.Contains(user.Role))
            throw new ForbiddenException("User does not allowed perform this action.");

        return await next(context);
    }
}