using Starter.Application.Tests.Setup;
using Starter.Domain.ReportingEntities;
using Starter.Domain.ReportingPeriods;
using Microsoft.EntityFrameworkCore;

namespace Starter.Application.Tests.ReportingPeriods;

[Collection(IntegrationTestCollection.Name)]
public sealed class ReportingPeriodPersistenceTests(PostgreSqlFixture fixture)
    : ApplicationIntegrationTest(fixture)
{
    [Fact]
    public async Task Save_WithDuplicateReferencePeriod_ShouldThrowDbUpdateException()
    {
        var reportingEntity = ReportingEntity.Create(
            "Aurora Financial Group",
            new ReportingEntityCode("AFG"));

        _dbContext.ReportingEntities.Add(reportingEntity);
        await _dbContext.SaveChangesAsync();

        var referencePeriod = new ReferencePeriod(2026, 9);

        var firstReportingPeriod = ReportingPeriod.Create(
            reportingEntity.Id,
            referencePeriod);

        _dbContext.ReportingPeriods.Add(firstReportingPeriod);
        await _dbContext.SaveChangesAsync();

        var secondReportingPeriod = ReportingPeriod.Create(
            reportingEntity.Id,
            referencePeriod);

        _dbContext.ReportingPeriods.Add(secondReportingPeriod);

        Task action() => _dbContext.SaveChangesAsync();

        await Assert.ThrowsAsync<DbUpdateException>(action);
    }

    [Fact]
    public async Task ReportingPeriod_ShouldBeMaterializedFromDatabase()
    {
        var entity = ReportingEntity.Create(
            "Test Entity",
            new ReportingEntityCode("TEST"));

        _dbContext.ReportingEntities.Add(entity);

        var period = ReportingPeriod.Create(
            entity.Id,
            new ReferencePeriod(2026, 9));

        _dbContext.ReportingPeriods.Add(period);

        await _dbContext.SaveChangesAsync();

        _dbContext.ChangeTracker.Clear();

        var persistedPeriod = await _dbContext.ReportingPeriods.SingleAsync(x => x.Id == period.Id);

        Assert.Equal(period.Id, persistedPeriod.Id);
        Assert.Equal(entity.Id, persistedPeriod.ReportingEntityId);
        Assert.Equal(new ReferencePeriod(2026, 9), persistedPeriod.ReferencePeriod);
        Assert.Equal(ReportingStatus.Draft, persistedPeriod.Status);
    }
}
