using Domain.Entities;

namespace Domain.Interfaces;

public interface  IProjectRepository : IRepository<ProjectEntity>
{
    Task<IEnumerable<ProjectEntity>> GetProjectByUserIdAsync(int userId);
}