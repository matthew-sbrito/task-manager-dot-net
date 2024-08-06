using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TaskManager.Api.Filters;

public class RequiredHeaderParameterFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "x-user-id",
            Description = "User id for authentication",
            In = ParameterLocation.Header,
            Required = false,
            AllowEmptyValue = false
        });
    }
}