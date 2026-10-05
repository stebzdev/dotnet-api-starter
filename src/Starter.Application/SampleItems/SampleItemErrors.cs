using Starter.Application.Common.Results;

namespace Starter.Application.SampleItems;

public static class SampleItemErrors
{
    public static readonly Error AlreadyExists = new(
        "SampleItem.AlreadyExists",
        "A sample item with the specified code already exists.",
        ErrorType.Conflict);

    public static readonly Error NotFound = new(
    "SampleItem.NotFound",
    "Sample item was not found.",
    ErrorType.NotFound);
}
