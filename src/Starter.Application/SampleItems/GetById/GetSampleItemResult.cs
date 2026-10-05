using Starter.Domain.SampleItems;

namespace Starter.Application.SampleItems.GetById;

public sealed record GetSampleItemResult(SampleItemId Id, string Name, SampleItemCode Code);
