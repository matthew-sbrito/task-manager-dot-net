using Domain.Entities;
using Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options) : DbContext(options)
{
    public virtual DbSet<UserEntity> Users { get; set; } = null!;
    public virtual DbSet<ProjectEntity> Projects { get; set; } = null!;
    public virtual DbSet<TaskEntity> Tasks { get; set; } = null!;
    
    public override int SaveChanges()
    {
        UpdateAuditableEntities();
        return base.SaveChanges();
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditableEntities();
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("public");

        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectEntityConfiguration());
        modelBuilder.ApplyConfiguration(new TaskEntityConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }

    private void UpdateAuditableEntities()
    {
        var modifiedEntries = ChangeTracker
            .Entries<AuditableEntity>()
            .Where(x => x.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in modifiedEntries)
        {
            var entity = entry.Entity;
            var now = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = now;
            }
            else
            {
                Entry(entity).Property(x => x.CreatedAt).IsModified = false;
            }

            if (entry.State == EntityState.Modified)
            {
                entity.UpdatedAt = now;
            }
            else
            {
                Entry(entity).Property(x => x.UpdatedAt).IsModified = false;
            }
        }
    }
}
