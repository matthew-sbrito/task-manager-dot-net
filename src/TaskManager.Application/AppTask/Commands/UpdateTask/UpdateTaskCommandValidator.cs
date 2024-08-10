using FluentValidation;
using TaskManager.Application.Common.Extensions;
using TaskManager.Shared.Helpers;

namespace TaskManager.Application.AppTask.Commands.UpdateTask;

public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskCommandValidator()
    {
        RuleFor(e => e.Body.Title)
            .Required("title", 5, 50);

        RuleFor(e => e.Body.Description)
            .Required("description", 5, 255);

        RuleFor(e => e.Body.Status)
            .IsInEnum()
            .WithMessage("The field status is invalid.");

        RuleFor(e => e.Body.DueDate)
            .NotEmpty()
            .WithMessage("The field due date is required.")
            .GreaterThan(DateTimeHelper.UtcNow())
            .WithMessage("The field due date must be greater than now.");
    }
}