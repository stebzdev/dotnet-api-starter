using Starter.Domain.Common;

namespace Starter.Domain.SampleItems;

public sealed class SampleItem
{
    public SampleItemId Id { get; private set; }

    public string Name { get; private set; } = null!;

    public SampleItemCode Code { get; private set; }

    private SampleItem()
    {
    }

    private SampleItem(SampleItemId id, string name, SampleItemCode code)
    {
        Id = id;
        Name = name;
        Code = code;
    }

    public static SampleItem Create(string name, SampleItemCode code)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Sample entity name cannot be empty.");
        }

        return new SampleItem(SampleItemId.New(), name.Trim(), code);
    }
}
