namespace TaskManager.Api.IntegrationTests.Common.IntegrationApplicationFactory;

[CollectionDefinition(nameof(IntegrationTestFactoryCollection), DisableParallelization = true)]
public class IntegrationTestFactoryCollection : ICollectionFixture<IntegrationTestFactory> { }