using Starter.Application.Common.CQRS;
using Starter.Domain.SampleItems;

namespace Starter.Application.SampleItems.Create;

public sealed record CreateSampleItemCommand(
    string Name,
    SampleItemCode Code)
    : ICommand<CreateSampleItemResult>;
