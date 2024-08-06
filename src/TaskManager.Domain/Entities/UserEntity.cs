using TaskManager.Shared.Enums;

namespace TaskManager.Domain.Entities;

public class UserEntity : AuditableEntity
{
    public string Name { get; set; } = null!;
    public UserRole Role { get; set; }
}