using TaskManager.Shared.Enums;

namespace TaskManager.Application.Contracts.AppTask;

public record UpdateTaskRequest(
    string Title,
    string Description,
    TaskEntityStatus Status,
    DateTime DueDate
);