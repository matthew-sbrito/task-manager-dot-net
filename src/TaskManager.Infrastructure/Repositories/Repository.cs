using TaskManager.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Shared.Helpers;

namespace TaskManager.Infrastructure.Repositories;

public abstract class Repository<TEntity>(DbContext context) : IRepository<TEntity> where TEntity : AuditableEntity
{
    protected DbSet<TEntity> DbSet => context.Set<TEntity>();

    public async Task<TEntity?> GetByIdAsync(int id, IEnumerable<string>? includes = null)
    {
        includes ??= [];

        return await DbSet
            .ApplyIncludes(includes)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<TEntity>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await DbSet.AddRangeAsync(entities);
    }

    public Task UpdateAsync(TEntity entity)
    {
        DbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task UpdateRangeAsync(IEnumerable<TEntity> entities)
    {
        DbSet.UpdateRange(entities);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity entity, int? deletedByUserId = null)
    {
        entity.DeletedAt = DateTimeHelper.UtcNow();
        entity.DeletedByUserId = deletedByUserId;
        DbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(IEnumerable<TEntity> entities, int? deletedByUserId = null)
    {
        var updatedEntities = entities
            .Select(entity =>
            {
                entity.DeletedByUserId = deletedByUserId;
                entity.DeletedAt = DateTimeHelper.UtcNow();
                return entity;
            });

        DbSet.UpdateRange(updatedEntities);
        return Task.CompletedTask;
    }
}