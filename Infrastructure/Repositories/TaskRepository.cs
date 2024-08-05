using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;
using Domain.Entities;

namespace Infrastructure.Repositories;

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