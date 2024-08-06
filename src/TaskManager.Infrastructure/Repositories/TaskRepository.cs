using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Infrastructure.Repositories;

public class TaskRepository(DbContext context) : Repository<TaskEntity>(context), ITaskRepository
{
    public async Task<IEnumerable<TaskEntity>> GetTasksByProjectIdAsync(int projectId, CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(x => x.ProjectId == projectId).ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountTasksByProjectIdAsync(int projectId, CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(x => x.ProjectId == projectId).CountAsync(cancellationToken);
    }
}