using Microsoft.EntityFrameworkCore;
using Starter.Application.Tests.Setup;
using Starter.Domain.SampleItems;

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

        DbContext.SampleItems.Add(item);
        await DbContext.SaveChangesAsync();

        DbContext.ChangeTracker.Clear();

        var persistedItem = await DbContext.SampleItems.SingleAsync(x => x.Id == item.Id);

        Assert.Equal(item.Id, persistedItem.Id);
        Assert.Equal(item.Name, persistedItem.Name);
        Assert.Equal(item.Code, persistedItem.Code);
    }
}
