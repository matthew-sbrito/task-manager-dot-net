using ErrorOr;
using MediatR;
using TaskManager.Application.Contracts.AppTask;
using TaskManager.Domain.ORM;

namespace TaskManager.Application.AppTask.Queries.ListTask;

public class ListTaskQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<ListTaskQuery, ErrorOr<List<TaskResponse>>>
{
    public async Task<ErrorOr<List<TaskResponse>>> Handle(ListTaskQuery request, CancellationToken cancellationToken)
    {
        var tasks = await unitOfWork
            .TaskRepository
            .GetTasksByProjectIdAsync(request.ProjectId, cancellationToken);

        return tasks
            .ConvertAll(TaskResponse.FromEntity);
    }
}