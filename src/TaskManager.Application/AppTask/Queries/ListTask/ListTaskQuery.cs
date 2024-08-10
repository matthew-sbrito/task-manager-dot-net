using ErrorOr;
using TaskManager.Application.Common.Security.Request;
using TaskManager.Application.Contracts.AppTask;

namespace TaskManager.Application.AppTask.Queries.ListTask;

public record ListTaskQuery(int AuthenticatedUserId, int ProjectId)
    : IAuthorizedRequest<ErrorOr<List<TaskResponse>>>;