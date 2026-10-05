using Starter.Application.Tests.Setup;
using Starter.Domain.ReportingEntities;
using Microsoft.EntityFrameworkCore;

namespace Starter.Application.Tests.ReportingEntities;

[Collection(IntegrationTestCollection.Name)]
public class ReportingEntityPersistenceTests(PostgreSqlFixture fixture) : ApplicationIntegrationTest(fixture)
{
    [Fact]
    public async Task ReportingEntity_ShouldBeMaterializedFromDatabase()
    {
        var entity = ReportingEntity.Create(
            "Test Entity",
            new ReportingEntityCode("TEST"));

        _dbContext.ReportingEntities.Add(entity);
        await _dbContext.SaveChangesAsync();

        _dbContext.ChangeTracker.Clear();

        var persistedEntity = await _dbContext.ReportingEntities.SingleAsync(x => x.Id == entity.Id);

        Assert.Equal(entity.Id, persistedEntity.Id);
        Assert.Equal(entity.Name, persistedEntity.Name);
        Assert.Equal(entity.Code, persistedEntity.Code);
    }
}
