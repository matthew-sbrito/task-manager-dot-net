namespace Domain.Entities;

public class UserProjectEntity
{
    public int UserId { get; set; }
    public UserEntity User { get; set; } = null!;
    public int ProjectId { get; set; }
    public ProjectEntity Project { get; set; } = null!;
}