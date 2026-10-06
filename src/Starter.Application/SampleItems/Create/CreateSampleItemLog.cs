using Starter.Domain.SampleItems;
using Microsoft.Extensions.Logging;

namespace Starter.Application.SampleItems.Create;

internal static partial class CreateSampleItemLog
{
    [LoggerMessage(Level = LogLevel.Warning, Message = "Sample item with code {SampleItemCode} already exists")]
    public static partial void AlreadyExists(ILogger logger, string SampleItemCode);

    [LoggerMessage(Level = LogLevel.Information, Message = "Sample item {SampleItemId} created with code {SampleItemCode}")]
    public static partial void Created(ILogger logger, Guid SampleItemId, string SampleItemCode);
}
