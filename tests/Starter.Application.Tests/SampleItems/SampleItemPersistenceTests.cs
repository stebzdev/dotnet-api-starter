using Starter.Application.Tests.Setup;
using Starter.Domain.SampleItems;
using Microsoft.EntityFrameworkCore;

namespace Starter.Application.Tests.SampleItems;

[Collection(IntegrationTestCollection.Name)]
public class SampleItemPersistenceTests(PostgreSqlFixture fixture) : ApplicationIntegrationTest(fixture)
{
    [Fact]
    public async Task SampleItem_ShouldBeMaterializedFromDatabase()
    {
        var item = SampleItem.Create(
            "Test Item",
            new SampleItemCode("TEST"));

        _dbContext.SampleItems.Add(item);
        await _dbContext.SaveChangesAsync();

        _dbContext.ChangeTracker.Clear();

        var persistedItem = await _dbContext.SampleItems.SingleAsync(x => x.Id == item.Id);

        Assert.Equal(item.Id, persistedItem.Id);
        Assert.Equal(item.Name, persistedItem.Name);
        Assert.Equal(item.Code, persistedItem.Code);
    }
}
