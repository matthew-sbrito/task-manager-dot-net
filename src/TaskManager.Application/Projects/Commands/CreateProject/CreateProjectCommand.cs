using ErrorOr;
using TaskManager.Application.Common.Security.Request;
using TaskManager.Application.Contracts.Projects;

namespace TaskManager.Application.Projects.Commands.CreateProject;

public record CreateProjectCommand(
    int AuthenticatedUserId,
    string Title,
    string Description
) : IAuthorizedRequest<ErrorOr<ProjectResponse>>;