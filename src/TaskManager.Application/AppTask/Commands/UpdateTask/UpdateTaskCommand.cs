using ErrorOr;
using TaskManager.Application.Common.Security.Request;
using TaskManager.Application.Contracts.AppTask;

namespace TaskManager.Application.AppTask.Commands.UpdateTask;

public record UpdateTaskCommand(
    int AuthenticatedUserId,
    int TaskId,
    UpdateTaskRequest Body
) : IAuthorizedRequest<ErrorOr<TaskResponse>>;