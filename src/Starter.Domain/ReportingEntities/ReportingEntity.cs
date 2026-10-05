using Starter.Domain.Common;

namespace Starter.Domain.ReportingEntities;

public sealed class ReportingEntity
{
    public ReportingEntityId Id { get; private set; }

    public string Name { get; private set; } = null!;

    public ReportingEntityCode Code { get; private set; }

    private ReportingEntity()
    {
    }

    private ReportingEntity(ReportingEntityId id, string name, ReportingEntityCode code)
    {
        Id = id;
        Name = name;
        Code = code;
    }

    public static ReportingEntity Create(string name, ReportingEntityCode code)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Reporting entity name cannot be empty.");
        }

        return new ReportingEntity(ReportingEntityId.New(), name.Trim(), code);
    }
}
