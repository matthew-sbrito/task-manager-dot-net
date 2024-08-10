using ErrorOr;
using MediatR;
using TaskManager.Domain.ORM;

namespace TaskManager.Application.AppTask.Commands.DeleteTask;

public class DeleteTaskCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteTaskCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var taskEntity = await unitOfWork.TaskRepository.GetByIdAsync(request.TaskId);

        if (taskEntity is null)
        {
            return Error.NotFound(description: "Task not found.");
        }

        await unitOfWork.TaskRepository.DeleteAsync(taskEntity, request.AuthenticatedUserId);
        await unitOfWork.SaveAsync();

        return new Success();
    }
}