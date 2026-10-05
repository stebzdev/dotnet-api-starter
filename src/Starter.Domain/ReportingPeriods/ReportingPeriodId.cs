
namespace Starter.Domain.ReportingPeriods;

public readonly record struct ReportingPeriodId(Guid Value)
{
    public static ReportingPeriodId New() => new(Guid.NewGuid());
}
