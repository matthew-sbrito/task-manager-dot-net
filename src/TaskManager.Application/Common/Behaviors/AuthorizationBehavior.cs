using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Application.Common.Security.Request;
using TaskManager.Shared.Enums;

namespace TaskManager.Application.Common.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse>(
    IAuthenticatedUserProvider authenticatedUserProvider,
    IAuthorizationUserService authorizationUserService
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IAuthorizedRequest<TResponse>
    where TResponse : IErrorOr
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var user = await authenticatedUserProvider.GetCurrentUser();

        if (user.IsError)
            return (dynamic)user.Errors;

        var authorizationAttributes = request.GetType()
            .GetCustomAttributes(true)
            .Where(x => x.GetType() == typeof(AuthorizeAttribute))
            .Cast<AuthorizeAttribute>()
            .ToList();

        if (authorizationAttributes.Count == 0)
        {
            return await next();
        }

        var roles = authorizationAttributes
            .SelectMany(x => x.Roles?.Split(",") ?? [])
            .Select(Enum.Parse<UserRole>)
            .ToList();

        var canAccess = authorizationUserService.UserCanAccess(user.Value, roles);

        return canAccess.IsError ? (dynamic)canAccess.Errors : await next();
    }
}