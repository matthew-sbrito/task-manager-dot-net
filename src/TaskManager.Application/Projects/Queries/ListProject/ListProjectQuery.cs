using ErrorOr;
using TaskManager.Application.Common.Security.Request;
using TaskManager.Application.Contracts.Projects;

namespace TaskManager.Application.Projects.Queries.ListProject;

public record ListProjectQuery(int AuthenticatedUserId) : IAuthorizedRequest<ErrorOr<List<ProjectResponse>>>;