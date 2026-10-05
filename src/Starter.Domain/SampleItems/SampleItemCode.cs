using Starter.Domain.Common;

namespace Starter.Domain.SampleItems;

public readonly record struct SampleItemCode
{
    public const string InvalidCodeMessage = "Entity code cannot be null or whitespace.";

    public string Value { get; }

    public SampleItemCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException(InvalidCodeMessage);
        }

        Value = value.Trim();
    }
}
