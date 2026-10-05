
namespace Starter.Domain.ReportingEntities;

public readonly record struct ReportingEntityId(Guid Value)
{
    public static ReportingEntityId New() => new(Guid.NewGuid());
}
