using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Infrastructure.Repositories;

public class ProjectRepository(DbContext context) : Repository<ProjectEntity>(context), IProjectRepository
{
    public async Task<List<ProjectEntity>> GetProjectByUserIdAsync(int userId)
    {
        return await DbSet
            .Where(x => x.CreatedByUserId == userId)
            .ToListAsync();
    }
}