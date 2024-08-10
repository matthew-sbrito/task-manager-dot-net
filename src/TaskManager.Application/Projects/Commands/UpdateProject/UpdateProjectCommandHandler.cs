using ErrorOr;
using MediatR;
using TaskManager.Application.Contracts.Projects;
using TaskManager.Domain.ORM;

namespace TaskManager.Application.Projects.Commands.UpdateProject;

public class UpdateProjectCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateProjectCommand, ErrorOr<ProjectResponse>>
{
    public async Task<ErrorOr<ProjectResponse>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var projectEntity = await unitOfWork.ProjectRepository.GetByIdAsync(request.ProjectId);

        if (projectEntity is null)
            return Error.NotFound(description: "Project not found.");
        
        projectEntity.Title = request.Title;
        projectEntity.Description = request.Description;
        projectEntity.UpdatedByUserId = request.AuthenticatedUserId;
        
        await unitOfWork.ProjectRepository.UpdateAsync(projectEntity);
        await unitOfWork.SaveAsync();

        return ProjectResponse.FromEntity(projectEntity);
    }
}