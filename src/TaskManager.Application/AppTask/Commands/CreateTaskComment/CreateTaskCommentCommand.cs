using ErrorOr;
using TaskManager.Application.Common.Security.Request;
using TaskManager.Application.Contracts.AppTask;

namespace TaskManager.Application.AppTask.Commands.CreateTaskComment;

public record CreateTaskCommentCommand(
    int AuthenticatedUserId,
    int TaskId,
    CreateTaskCommentRequest Body
) : IAuthorizedRequest<ErrorOr<TaskCommentResponse>>;