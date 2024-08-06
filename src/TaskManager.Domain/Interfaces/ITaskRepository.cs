using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

public interface ITaskRepository : IRepository<TaskEntity>
{
    Task<IEnumerable<TaskEntity>>
        GetTasksByProjectIdAsync(int projectId, CancellationToken cancellationToken = default);

    Task<int> GetCountTasksByProjectIdAsync(int projectId, CancellationToken cancellationToken = default);
}