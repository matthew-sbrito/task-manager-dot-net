using ErrorOr;
using TaskManager.Application.Common.Security.Request;
using TaskManager.Application.Contracts.AppTask;

namespace TaskManager.Application.AppTask.Commands.CreateTask;

public record CreateTaskCommand(
    int AuthenticatedUserId,
    int ProjectId,
    CreateTaskRequest Body
) : IAuthorizedRequest<ErrorOr<TaskResponse>>;