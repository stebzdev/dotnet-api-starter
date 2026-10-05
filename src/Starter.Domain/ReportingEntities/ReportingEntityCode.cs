using Starter.Domain.Common;

namespace Starter.Domain.ReportingEntities;

public readonly record struct ReportingEntityCode
{
    public const string InvalidCodeMessage = "Entity code cannot be null or whitespace.";

    public string Value { get; }

    public ReportingEntityCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException(InvalidCodeMessage);
        }

        Value = value.Trim();
    }
}
