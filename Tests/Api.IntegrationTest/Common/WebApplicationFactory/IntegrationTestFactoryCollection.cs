namespace Tests.Api.IntegrationTest.Common.WebApplicationFactory;

[CollectionDefinition(nameof(IntegrationTestFactoryCollection), DisableParallelization = true)]
public class IntegrationTestFactoryCollection : ICollectionFixture<IntegrationTestFactory> { }