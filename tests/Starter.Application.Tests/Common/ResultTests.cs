using Starter.Application.Common.Results;

namespace Starter.Application.Tests.Common;

public sealed class ResultTests
{
    [Fact]
    public void Error_WhenResultIsSuccess_ShouldThrowInvalidOperationException()
    {
        var result = Result<int>.Success(42);

        void Action() => _ = result.Error;

        Assert.Throws<InvalidOperationException>(Action);
    }

    [Fact]
    public void Value_WhenResultIsFailure_ShouldThrowInvalidOperationException()
    {
        var error = new Error(
            "Test.Error",
            "Test error.",
            ErrorType.Validation);

        var result = Result<int>.Failure(error);

        void Action() => _ = result.Value;

        Assert.Throws<InvalidOperationException>(Action);
    }
}
