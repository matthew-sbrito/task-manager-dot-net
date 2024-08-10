using ErrorOr;
using TaskManager.Application.Common.Security.Request;

namespace TaskManager.Application.AppTask.Commands.DeleteTask;

public record DeleteTaskCommand(
    int AuthenticatedUserId,
    int TaskId
) : IAuthorizedRequest<ErrorOr<Success>>;