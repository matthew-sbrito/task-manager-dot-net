using TaskManager.Shared.Enums;

namespace TaskManager.Application.Contracts.AppTask;

public record CreateTaskRequest(
    string Title,
    string Description,
    TaskPriority Priority,
    DateTime DueDate
);