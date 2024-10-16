using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Extensions;

public static class EntityTypeBuilderExtension
{
    public static void SetAuditableConfiguration<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : AuditableEntity
    {
        builder
            .Property(e => e.Id)
            .HasColumnName("id")
            .HasColumnType("serial")
            .UseSerialColumn()
            .ValueGeneratedOnAdd();

        builder
            .Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp without time zone")
            .IsRequired();

        builder.Property(e => e.CreatedByUserId)
            .HasColumnName("created_by_user_id");

        builder
            .HasOne(e => e.CreatedByUser)
            .WithMany()
            .HasForeignKey(e => e.CreatedByUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(e => e.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp without time zone");

        builder.Property(e => e.UpdatedByUserId)
            .HasColumnName("updated_by_user_id");
        
        builder
            .HasOne(e => e.UpdatedByUser)
            .WithMany()
            .HasForeignKey(e => e.UpdatedByUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(e => e.DeletedAt)
            .HasColumnName("deleted_at")
            .HasColumnType("timestamp without time zone");

        builder.Property(e => e.DeletedByUserId)
            .HasColumnName("deleted_by_user_id");
        
        builder
            .HasOne(e => e.DeletedByUser)
            .WithMany()
            .HasForeignKey(e => e.DeletedByUserId)
            .OnDelete(DeleteBehavior.Cascade);
    } 
}