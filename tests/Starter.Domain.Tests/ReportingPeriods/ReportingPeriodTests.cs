using Starter.Domain.Common;
using Starter.Domain.ReportingEntities;
using Starter.Domain.ReportingPeriods;

namespace Starter.Domain.Tests.ReportingPeriods;

public sealed class ReportingPeriodTests
{
    [Fact]
    public void Create_ShouldSetStatusToDraft()
    {
        var reportingEntityId = new ReportingEntityId(Guid.NewGuid());
        var referencePeriod = new ReferencePeriod(2026, 6);

        var reportingPeriod = ReportingPeriod.Create(
            reportingEntityId,
            referencePeriod);

        Assert.Equal(ReportingStatus.Draft, reportingPeriod.Status);
    }

    [Fact]
    public void Create_ShouldGenerateId()
    {
        var reportingPeriod = ReportingPeriod.Create(
            new ReportingEntityId(Guid.NewGuid()),
            new ReferencePeriod(2026, 6));

        Assert.NotEqual(Guid.Empty, reportingPeriod.Id.Value);
    }

    [Fact]
    public void Create_ShouldStoreReferencePeriod()
    {
        var reportingEntityId = new ReportingEntityId(Guid.NewGuid());
        var referencePeriod = new ReferencePeriod(2026, 6);

        var reportingPeriod = ReportingPeriod.Create(
            reportingEntityId,
            referencePeriod);

        Assert.Equal(reportingEntityId, reportingPeriod.ReportingEntityId);
        Assert.Equal(referencePeriod, reportingPeriod.ReferencePeriod);
    }

    [Fact]
    public void Create_WithEmptyReportingEntityId_ShouldThrow()
    {
        var referencePeriod = new ReferencePeriod(2026, 6);

        Action action = () => ReportingPeriod.Create(
            new ReportingEntityId(Guid.Empty),
            referencePeriod);

        Assert.Throws<DomainException>(action);
    }
}
