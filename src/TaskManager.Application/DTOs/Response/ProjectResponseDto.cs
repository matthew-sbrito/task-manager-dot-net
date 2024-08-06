namespace TaskManager.Application.DTOs.Response;

public class ProjectResponseDto : AuditableResponseDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
}