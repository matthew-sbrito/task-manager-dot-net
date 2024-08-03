using Api.Endpoints.Common;
using Api.Extensions;
using Common.Enums;

namespace Api.Endpoints;

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