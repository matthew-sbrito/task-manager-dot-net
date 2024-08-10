using TaskManager.Domain.Entities;

namespace TaskManager.Application.Contracts.Common;

public class AuditableResponse
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? CreatedByUserId { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? UpdatedByUserId { get; set; }

    public void FillFromEntity(AuditableEntity entity)
    {
        Id = entity.Id;
        CreatedAt = entity.CreatedAt;
        CreatedByUserId = entity.CreatedByUserId;
        UpdatedAt = entity.UpdatedAt;
        UpdatedByUserId = entity.UpdatedByUserId;
    }
}