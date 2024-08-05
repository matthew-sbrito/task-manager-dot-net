using Application.DTOs.Request;
using Application.Extensions;
using Common.Helpers;
using Domain.ORM;
using FluentValidation;

namespace Application.Validators;

public class CreateTaskValidator : AbstractValidator<CreateTaskDto>
{
    private const int MaxTaskPerProject = 20;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTaskValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(e => e.Title)
            .Required("title", 5, 50);

        RuleFor(e => e.Description)
            .Required("description", 5, 255);

        RuleFor(e => e.TaskPriority)
            .IsInEnum()
            .WithMessage("The field priority is invalid.");

        RuleFor(e => e.DueDate)
            .NotEmpty()
            .WithMessage("The field due date is required.")
            .GreaterThan(DateTimeHelper.UtcNow())
            .WithMessage("The field due date must be greater than now.");
        
        RuleFor(e => e.ProjectId)
            .MustAsync(HaveMaximumAllowed)
            .WithMessage($"The number max of task for a project is {MaxTaskPerProject}.");
    }

    private async Task<bool> HaveMaximumAllowed(int projectId, CancellationToken cancellationToken)
    {
        var count = await _unitOfWork.TaskRepository
            .GetCountTasksByProjectIdAsync(projectId, cancellationToken);

        return count < MaxTaskPerProject;
    }
}