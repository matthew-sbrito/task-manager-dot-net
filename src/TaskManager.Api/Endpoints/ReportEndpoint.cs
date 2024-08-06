using TaskManager.Api.Extensions;
using TaskManager.Api.Endpoints.Common;
using TaskManager.Shared.Enums;

namespace TaskManager.Api.Endpoints;

public class ReportEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/reports")
            .WithTags(EndpointConstants.Tags.Report)
            .RequireTaskManagerAuthorization(UserRole.Manager);

        // group.MapGet("");
    }
}