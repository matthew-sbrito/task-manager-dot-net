using FluentValidation;
using FluentValidation.Results;

namespace TaskManager.Application.Extensions;

public static class ValidationExtension
{
    public static IRuleBuilderOptions<T, string> Required<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        string fieldName,
        int? minLength = null,
        int? maxLength = null
    )
    {
        var ruleBuilderOptions = ruleBuilder
            .NotEmpty()
            .WithMessage($"The field {fieldName} is required.");

        if (minLength.HasValue)
        {
            ruleBuilderOptions = ruleBuilderOptions
                .MinimumLength(minLength.Value)
                .WithMessage($"The field {fieldName} must have minimum {minLength.Value} char.");
        }
        
        if (maxLength.HasValue)
        {
            ruleBuilderOptions = ruleBuilderOptions
                .MaximumLength(maxLength.Value)
                .WithMessage($"The field {fieldName} must have maximum {maxLength.Value} char.");
        }
        
        return ruleBuilderOptions;
    }

    public static string GetErrorsMessage(this ValidationResult validationResult)
    {
        return string.Join("\n", validationResult.Errors.Select(x => x.ErrorMessage));
    }
}