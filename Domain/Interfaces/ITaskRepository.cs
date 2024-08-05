using Domain.Entities;

namespace Domain.Interfaces;

public interface ITaskRepository : IRepository<TaskEntity>
{
    Task<IEnumerable<TaskEntity>>
        GetTasksByProjectIdAsync(int projectId, CancellationToken cancellationToken = default);

    Task<int> GetCountTasksByProjectIdAsync(int projectId, CancellationToken cancellationToken = default);
}