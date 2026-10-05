namespace Starter.Api.Tests.Setup;

[CollectionDefinition(Name)]
public sealed class ApiTestCollection : ICollectionFixture<ApiFixture>
{
    public const string Name = "ApiTests";
}
