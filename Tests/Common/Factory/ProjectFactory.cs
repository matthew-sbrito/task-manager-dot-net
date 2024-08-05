using Application.DTOs.Request;
using Application.DTOs.Response;
using Bogus;
using Bogus.Extensions;
using Common.Helpers;
using Domain.Entities;

namespace Tests.Common.Factory;

public static class ProjectFactory
{
    private static readonly Faker<ProjectRequestDto> ValidProjectRequestGenerator =
        new Faker<ProjectRequestDto>()
            .RuleFor(x => x.Title, f => f.Name.JobArea())
            .RuleFor(x => x.Description, f => f.Name.JobDescriptor());
    
    private static readonly Faker<ProjectRequestDto> InvalidProjectRequestGenerator =
        new Faker<ProjectRequestDto>()
            .RuleFor(x => x.Title, f => f.Name.JobArea().ClampLength(max: 4))
            .RuleFor(x => x.Description, f => f.Name.JobDescriptor().ClampLength(max: 4));

    public static ProjectRequestDto CreateValidPayload()
    {
        return ValidProjectRequestGenerator.Generate(1).First();
    }
    
    public static ICollection<ProjectRequestDto> CreateValidPayload(int count)
    {
        return ValidProjectRequestGenerator.Generate(count);
    } 
    
    public static ProjectRequestDto CreateInvalidPayload()
    {
        return InvalidProjectRequestGenerator.Generate(1).First();
    }

    public static ProjectEntity ToEntity(ProjectRequestDto request)
    {
        return new ProjectEntity
        {
            Id = 1,
            Title = request.Title,
            Description = request.Description,
            CreatedAt = DateTimeHelper.UtcNow(),
            CreatedByUserId = 1
        };
    }
    
    public static ProjectResponseDto ToResponse(ProjectEntity entity)
    {
        return new ProjectResponseDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            CreatedAt = entity.CreatedAt,
            CreatedByUserId = entity.CreatedByUserId
        };
    }
}