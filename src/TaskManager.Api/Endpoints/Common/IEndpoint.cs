namespace TaskManager.Api.Endpoints.Common;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}