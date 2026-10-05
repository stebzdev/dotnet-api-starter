using Starter.Application.Common.Results;
using HttpResults = Microsoft.AspNetCore.Http.Results;

namespace Starter.Api.Common.Http;

public static class ResultExtensions
{
    public static IResult ToProblem(this Error error) => error.Type switch
    {
        ErrorType.Validation => HttpResults.BadRequest(error),
        ErrorType.NotFound => HttpResults.NotFound(error),
        ErrorType.Conflict => HttpResults.Conflict(error),
        _ => throw new ArgumentOutOfRangeException(nameof(error), error.Type, "Unsupported error type.")
    };
}
