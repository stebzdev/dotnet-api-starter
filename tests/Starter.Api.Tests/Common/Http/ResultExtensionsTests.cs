using Starter.Application.Common.Results;
using Starter.Api.Common.Http;

namespace Starter.Api.Tests.Common.Http;

public sealed class ResultExtensionsTests
{
    [Fact]
    public void ToProblem_WhenErrorTypeIsUnsupported_ShouldThrowArgumentOutOfRangeException()
    {
        var unsupportedErrorType = (ErrorType)999;
        var error = new Error(
            "Test.Error",
            "Test error.",
            unsupportedErrorType);

        void Action() => error.ToProblem();

        var exception = Assert.Throws<ArgumentOutOfRangeException>(Action);

        Assert.Equal("error", exception.ParamName);
        Assert.Equal(unsupportedErrorType, exception.ActualValue);
    }
}
