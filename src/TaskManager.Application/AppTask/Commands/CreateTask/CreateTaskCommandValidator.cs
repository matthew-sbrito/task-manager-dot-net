using FluentValidation;
using TaskManager.Application.Common.Extensions;
using TaskManager.Shared.Helpers;

namespace TaskManager.Application.AppTask.Commands.CreateTask;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(e => e.Body.Title)
            .Required("title", 5, 50);

        RuleFor(e => e.Body.Description)
            .Required("description", 5, 255);

        RuleFor(e => e.Body.Priority)
            .IsInEnum()
            .WithMessage("The field priority is invalid.");

        RuleFor(e => e.Body.DueDate)
            .NotEmpty()
            .WithMessage("The field due date is required.")
            .GreaterThan(DateTimeHelper.UtcNow())
            .WithMessage("The field due date must be greater than now.");
    }
}