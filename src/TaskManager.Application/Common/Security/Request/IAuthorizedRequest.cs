using MediatR;

namespace TaskManager.Application.Common.Security.Request;

public interface IAuthorizedRequest<out T> : IRequest<T>
{
    int AuthenticatedUserId { get; }    
}