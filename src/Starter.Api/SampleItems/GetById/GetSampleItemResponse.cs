namespace Starter.Api.SampleItems.GetById;

public sealed record GetSampleItemResponse(
    Guid Id,
    string Name,
    string Code);
