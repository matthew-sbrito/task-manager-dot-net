using Common.Enums;

namespace Application.DTOs.Request;

public class CreateTaskDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public TaskPriority TaskPriority { get; set; }
    public DateTime DueDate { get; set; }
}