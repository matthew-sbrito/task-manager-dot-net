using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Extensions;

namespace Infrastructure.Configurations;

public class ProjectEntityConfiguration : IEntityTypeConfiguration<ProjectEntity>
{
    public void Configure(EntityTypeBuilder<ProjectEntity> builder)
    {
        builder.ToTable("projects");

        builder.SetAuditableConfiguration();
        
        builder.Property(e => e.Title)
            .HasMaxLength(55)
            .HasColumnName("name");
        
        builder.Property(e => e.Description)
            .HasMaxLength(255)
            .HasColumnName("description");

        builder.HasMany(e => e.Tasks)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}