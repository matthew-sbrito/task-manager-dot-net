using ErrorOr;
using MediatR;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Application.Contracts.AppTask;
using TaskManager.Domain.Entities;
using TaskManager.Domain.ORM;

namespace TaskManager.Application.AppTask.Commands.CreateTaskComment;

public class CreateTaskCommentCommandHandler(IUnitOfWork unitOfWork, ITaskHistoryService taskHistoryService)
    : IRequestHandler<CreateTaskCommentCommand, ErrorOr<TaskCommentResponse>>
{
    public async Task<ErrorOr<TaskCommentResponse>> Handle(CreateTaskCommentCommand request,
        CancellationToken cancellationToken)
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

            await taskHistoryService.RegisterHistory(user, task, request.Body);

            var comment = new TaskCommentEntity
            {
                Content = request.Body.Content,
                Task = task,
                CreatedByUserId = user.Id
            };

            await unitOfWork.TaskCommentRepository.AddAsync(comment);
            await unitOfWork.SaveAsync();

            await unitOfWork.CommitAsync();

            return TaskCommentResponse.FromEntity(comment);
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