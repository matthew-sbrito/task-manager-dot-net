using TaskManager.Shared.Enums;

namespace TaskManager.Application.DTOs.Request;

public class CreateTaskDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int ProjectId { get; set; }
    public TaskPriority TaskPriority { get; set; }
    public DateTime DueDate { get; set; }
}