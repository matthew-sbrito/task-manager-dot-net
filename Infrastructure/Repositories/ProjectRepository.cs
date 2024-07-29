using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProjectRepository(DbContext context) : Repository<ProjectEntity>(context), IProjectRepository
{
    public async Task<IEnumerable<ProjectEntity>> GetProjectByUserIdAsync(int userId)
    {
        return await DbSet
            .Where(x => x.CreatedByUserId == userId)
            .ToListAsync();
    }
}