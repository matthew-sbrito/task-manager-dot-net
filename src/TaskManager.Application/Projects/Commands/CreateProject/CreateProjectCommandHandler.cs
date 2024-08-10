using ErrorOr;
using MediatR;
using TaskManager.Application.Contracts.Projects;
using TaskManager.Domain.Entities;
using TaskManager.Domain.ORM;

namespace TaskManager.Application.Projects.Commands.CreateProject;

public class CreateProjectCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateProjectCommand, ErrorOr<ProjectResponse>>
{
    public async Task<ErrorOr<ProjectResponse>> Handle(
        CreateProjectCommand request,
        CancellationToken cancellationToken
    )
    {
        var projectEntity = new ProjectEntity
        {
            Title = request.Title,
            Description = request.Description,
            CreatedByUserId = request.AuthenticatedUserId
        };

        await unitOfWork.ProjectRepository.AddAsync(projectEntity);
        await unitOfWork.SaveAsync();

        return ProjectResponse.FromEntity(projectEntity);
    }
}