namespace Application.DTOs.Request;

public class CreateProjectDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
}