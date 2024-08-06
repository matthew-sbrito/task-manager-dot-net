using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

public interface  IProjectRepository : IRepository<ProjectEntity>
{
    Task<IEnumerable<ProjectEntity>> GetProjectByUserIdAsync(int userId);
}