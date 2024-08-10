using FluentValidation;
using TaskManager.Application.Common.Extensions;

namespace TaskManager.Application.Projects.Commands.UpdateProject;

public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(e => e.ProjectId)
            .NotEmpty()
            .WithMessage("The field project id cannot be empty.");
        
        RuleFor(e => e.Title)
            .Required("title", 5, 50);

        RuleFor(e => e.Description)
            .Required("description", 5, 255);
    }
}