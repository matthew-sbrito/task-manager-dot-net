using ErrorOr;
using MediatR;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Application.Contracts.AppTask;
using TaskManager.Domain.ORM;

namespace TaskManager.Application.AppTask.Commands.UpdateTask;

public class UpdateTaskCommandHandler(IUnitOfWork unitOfWork, ITaskHistoryService taskHistoryService) : IRequestHandler<UpdateTaskCommand, ErrorOr<TaskResponse>>
{
    public async Task<ErrorOr<TaskResponse>> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.UserRepository.GetByIdAsync(request.AuthenticatedUserId);
        var task = await unitOfWork.TaskRepository.GetByIdAsync(request.TaskId);

        if (user is null)
        {
            return Error.Unauthorized(description: "Invalid user id on authentication.");
        }
        
        if (task is null)
        {
            return Error.NotFound(description: "Task not found.");
        }

        try
        {
            await unitOfWork.BeginTransactionAsync();

            var registerHistoryResult = await taskHistoryService
                .RegisterHistory(user, task, request.Body);

            if (registerHistoryResult.IsError)
            {
                return registerHistoryResult.Errors;
            }

            task.Title = request.Body.Title;
            task.Description = request.Body.Description;
            task.Status = request.Body.Status;
            task.DueDate = request.Body.DueDate;
            task.UpdatedByUserId = user.Id;

            await unitOfWork.TaskRepository.UpdateAsync(task);
            await unitOfWork.SaveAsync();

            await unitOfWork.CommitAsync();

            return TaskResponse.FromEntity(task);
        }
        catch (Exception)
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
        finally
        {
            await unitOfWork.DisposeAsync();
        }
    }
}