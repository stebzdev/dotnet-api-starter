using Starter.Application.Common.Results;

namespace Starter.Application.ReportingEntities;

public static class ReportingEntityErrors
{
    public static readonly Error AlreadyExists = new(
        "ReportingEntity.AlreadyExists",
        "A reporting entity with the specified code already exists.",
        ErrorType.Conflict);

    public static readonly Error NotFound = new(
    "ReportingEntity.NotFound",
    "Reporting entity was not found.",
    ErrorType.NotFound);
}
