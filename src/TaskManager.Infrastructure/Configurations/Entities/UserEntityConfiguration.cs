using TaskManager.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Configurations.Entities;

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("users");

        builder.SetAuditableConfiguration();

        builder.Property(e => e.Name)
            .HasMaxLength(255)
            .HasColumnName("name")
            .IsRequired();

        builder.Property(e => e.Role)
            .HasColumnName("role")
            .IsRequired();
    }
}