using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

public interface  IProjectRepository : IRepository<ProjectEntity>
{
    Task<List<ProjectEntity>> GetProjectByUserIdAsync(int userId);
}