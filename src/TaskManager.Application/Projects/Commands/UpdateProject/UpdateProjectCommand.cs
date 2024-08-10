using ErrorOr;
using TaskManager.Application.Common.Security.Request;
using TaskManager.Application.Contracts.Projects;

namespace TaskManager.Application.Projects.Commands.UpdateProject;

public record UpdateProjectCommand(
    int AuthenticatedUserId,
    int ProjectId,
    string Title,
    string Description
) : IAuthorizedRequest<ErrorOr<ProjectResponse>>;