using Application.DTOs.Request;
using Application.Extensions;
using FluentValidation;

namespace Application.Validators;

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