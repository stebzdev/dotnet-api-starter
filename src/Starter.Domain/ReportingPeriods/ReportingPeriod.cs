using Starter.Domain.Common;
using Starter.Domain.ReportingEntities;

namespace Starter.Domain.ReportingPeriods;

public sealed class ReportingPeriod
{
    public ReportingPeriodId Id { get; private set; }

    public ReportingEntityId ReportingEntityId { get; private set; }

    public ReferencePeriod ReferencePeriod { get; private set; }

    public ReportingStatus Status { get; private set; }

    private ReportingPeriod()
    {
    }

    private ReportingPeriod(
        ReportingPeriodId id,
        ReportingEntityId reportingEntityId,
        ReferencePeriod referencePeriod)
    {
        Id = id;
        ReportingEntityId = reportingEntityId;
        ReferencePeriod = referencePeriod;
        Status = ReportingStatus.Draft;
    }

    public static ReportingPeriod Create(ReportingEntityId reportingEntityId, ReferencePeriod referencePeriod)
    {
        if (reportingEntityId.Value == Guid.Empty)
        {
            throw new DomainException("Reporting entity identifier cannot be empty.");
        }

        return new ReportingPeriod(
            ReportingPeriodId.New(),
            reportingEntityId,
            referencePeriod);
    }
}
