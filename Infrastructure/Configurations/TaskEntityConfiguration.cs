using Domain.Entities;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class TaskEntityConfiguration : IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.ToTable("tasks");
        
        builder.SetAuditableConfiguration();
        
        builder.Property(e => e.Title)
            .HasMaxLength(55)
            .HasColumnName("name")
            .IsRequired();
        
        builder.Property(e => e.Description)
            .HasMaxLength(255)
            .HasColumnName("description")
            .IsRequired();

        builder.Property(e => e.Status)
            .HasColumnName("status")
            .IsRequired();
        
        builder.Property(e => e.Priority)
            .HasColumnName("priority")
            .IsRequired();
        
        builder.Property(e => e.ProjectId)
            .HasColumnName("project_id")
            .IsRequired();
        
        builder.Property(e => e.DueDate)
            .HasColumnName("due_date")
            .HasColumnType("timestamp without time zone")
            .IsRequired();

        builder.HasOne(e => e.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(e => e.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}