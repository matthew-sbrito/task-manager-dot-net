using ErrorOr;
using TaskManager.Application.Common.Security.Request;

namespace TaskManager.Application.Projects.Commands.DeleteProject;

public record DeleteProjectCommand(
    int AuthenticatedUserId,
    int ProjectId
) : IAuthorizedRequest<ErrorOr<Success>>;