using ErrorOr;
using MediatR;
using TaskManager.Domain.ORM;
using TaskManager.Shared.Enums;

namespace TaskManager.Application.Projects.Commands.DeleteProject;

public class DeleteProjectCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteProjectCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var projectEntity = await unitOfWork.ProjectRepository.GetByIdAsync(request.ProjectId, ["Tasks"]);

        if (projectEntity is null)
        {
            return Error.NotFound(description: "Project not found.");
        }

        if (projectEntity.Tasks.Any(x => x.Status != TaskEntityStatus.Concluded))
        {
            return Error.Validation(description: "Project cannot be remove because has pending task.");
        }

        await unitOfWork.ProjectRepository.DeleteAsync(projectEntity);
        await unitOfWork.SaveAsync();

        return new Success();
    }
}