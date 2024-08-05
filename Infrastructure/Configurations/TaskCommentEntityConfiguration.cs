using Domain.Entities;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class TaskCommentEntityConfiguration : IEntityTypeConfiguration<TaskCommentEntity>
{
    public void Configure(EntityTypeBuilder<TaskCommentEntity> builder)
    {
        builder.ToTable("task_comments");
        
        builder.SetAuditableConfiguration();
        
        builder.Property(e => e.Content)
            .HasColumnName("content")
            .IsRequired();
        
        builder.Property(e => e.TaskId)
            .HasColumnName("task_id")
            .IsRequired();

        builder.HasOne(e => e.Task)
            .WithMany(p => p.Comments)
            .HasForeignKey(e => e.TaskId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}