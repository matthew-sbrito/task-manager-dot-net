using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : AuditableEntity
{
    Task<TEntity?> GetByIdAsync(int id, IEnumerable<string>? includes = null);
    Task<IEnumerable<TEntity>> GetAll();
    Task AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);
    Task UpdateAsync(TEntity entity);
    Task UpdateRangeAsync(IEnumerable<TEntity> entities);
    Task DeleteAsync(TEntity entity, int? deletedByUserId = null);
    Task DeleteRangeAsync(IEnumerable<TEntity> entities, int? deletedByUserId = null);
}