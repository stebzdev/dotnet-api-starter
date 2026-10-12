namespace Starter.Api.SampleItems.GetById;

internal sealed record GetSampleItemResponse(
    Guid Id,
    string Name,
    string Code);
