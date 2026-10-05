namespace Starter.Application.Tests.Setup;

[CollectionDefinition(Name)]
public sealed class IntegrationTestCollection : ICollectionFixture<PostgreSqlFixture>
{
    public const string Name = "IntegrationTests";
}
