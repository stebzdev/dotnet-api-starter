using Starter.Application.Common.CQRS;
using Starter.Domain.SampleItems;

namespace Starter.Application.SampleItems.GetById;

public sealed record GetSampleItemQuery(SampleItemId Id) : IQuery<GetSampleItemResult>;
