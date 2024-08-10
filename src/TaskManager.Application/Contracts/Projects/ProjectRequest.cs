namespace TaskManager.Application.Contracts.Projects;

public record ProjectRequest(
    string Title,
    string Description
);