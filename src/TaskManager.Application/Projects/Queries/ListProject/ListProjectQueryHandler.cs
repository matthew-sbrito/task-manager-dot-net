using ErrorOr;
using MediatR;
using TaskManager.Application.Contracts.Projects;
using TaskManager.Domain.ORM;

namespace TaskManager.Application.Projects.Queries.ListProject;

public class ListProjectQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<ListProjectQuery, ErrorOr<List<ProjectResponse>>>
{
    public async Task<ErrorOr<List<ProjectResponse>>> Handle(ListProjectQuery request, CancellationToken cancellationToken)
    {
        var projects = await unitOfWork.ProjectRepository
            .GetProjectByUserIdAsync(request.AuthenticatedUserId);

        return projects.ConvertAll(ProjectResponse.FromEntity);
    }
}