namespace Application.DTOs.Response;

public class AuditableResponseDto
{
    public int Id { get; set; }
    public int CreatedAt { get; set; }
    public int CreatedByUserId { get; set; }
    public int UpdatedAt { get; set; }
    public int UpdatedByUserId { get; set; }
}