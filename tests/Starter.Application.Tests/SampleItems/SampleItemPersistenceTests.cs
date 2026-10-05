using Starter.Application.Tests.Setup;
using Starter.Domain.SampleItems;
using Microsoft.EntityFrameworkCore;

namespace Starter.Application.Tests.SampleItems;

[Collection(IntegrationTestCollection.Name)]
public class SampleItemPersistenceTests(PostgreSqlFixture fixture) : ApplicationIntegrationTest(fixture)
{
    [Fact]
    public async Task SampleEntity_ShouldBeMaterializedFromDatabase()
    {
        var entity = SampleItem.Create(
            "Test Entity",
            new SampleItemCode("TEST"));

        _dbContext.SampleItems.Add(entity);
        await _dbContext.SaveChangesAsync();

        _dbContext.ChangeTracker.Clear();

        var persistedEntity = await _dbContext.SampleItems.SingleAsync(x => x.Id == entity.Id);

        Assert.Equal(entity.Id, persistedEntity.Id);
        Assert.Equal(entity.Name, persistedEntity.Name);
        Assert.Equal(entity.Code, persistedEntity.Code);
    }
}
