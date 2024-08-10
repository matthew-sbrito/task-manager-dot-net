using FluentValidation;
using TaskManager.Application.Common.Extensions;

namespace TaskManager.Application.Projects.Commands.CreateProject;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(e => e.Title)
            .Required("title", 5, 50);

        RuleFor(e => e.Description)
            .Required("description", 5, 255);
    }
}