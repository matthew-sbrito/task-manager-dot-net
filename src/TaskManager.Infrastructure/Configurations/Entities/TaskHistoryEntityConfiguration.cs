using TaskManager.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Configurations.Entities;

public class TaskHistoryEntityConfiguration : IEntityTypeConfiguration<TaskHistoryEntity>
{
    public void Configure(EntityTypeBuilder<TaskHistoryEntity> builder)
    {
        builder.ToTable("task_histories");
        
        builder.SetAuditableConfiguration();
        
        builder.Property(e => e.Details)
            .HasColumnName("details")
            .IsRequired();
        
        builder.Property(e => e.TaskId)
            .HasColumnName("task_id")
            .IsRequired();

        builder.HasOne(e => e.Task)
            .WithMany(p => p.Histories)
            .HasForeignKey(e => e.TaskId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}