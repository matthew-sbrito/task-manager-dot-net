using FluentValidation;
using TaskManager.Application.DTOs.Request;
using TaskManager.Application.Extensions;

namespace TaskManager.Application.Validators;

public class ProjectRequestValidator : AbstractValidator<ProjectRequestDto>
{
    
    public ProjectRequestValidator()
    {
        RuleFor(e => e.Title)
            .Required("title", 5, 50);

        RuleFor(e => e.Description)
            .Required("description", 5, 255);
    }
}