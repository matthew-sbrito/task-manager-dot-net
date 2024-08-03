using Common.Enums;

namespace Application.DTOs.Request;

public class UpdateTaskDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public TaskEntityStatus Status { get; set; }
    public DateTime DueDate { get; set; }
}