using Bogus.Extensions;
using Bogus;
using TaskManager.Application.Contracts.Projects;

namespace TaskManager.TestCommon.Project;

public static class ProjectFactory
{
    private static readonly Faker<ProjectRequest> ValidProjectRequestGenerator =
        new Faker<ProjectRequest>()
            .RuleFor(x => x.Title, f => f.Name.JobArea())
            .RuleFor(x => x.Description, f => f.Name.JobDescriptor());

    private static readonly Faker<ProjectRequest> InvalidProjectRequestGenerator =
        new Faker<ProjectRequest>()
            .RuleFor(x => x.Title, f => f.Name.JobArea().ClampLength(max: 4))
            .RuleFor(x => x.Description, f => f.Name.JobDescriptor().ClampLength(max: 4));

    public static ProjectRequest CreateValidPayload()
    {
        return ValidProjectRequestGenerator.Generate(1).First();
    }

    public static ICollection<ProjectRequest> CreateValidPayload(int count)
    {
        return ValidProjectRequestGenerator.Generate(count);
    }

    public static ProjectRequest CreateInvalidPayload()
    {
        return InvalidProjectRequestGenerator.Generate(1).First();
    }
}