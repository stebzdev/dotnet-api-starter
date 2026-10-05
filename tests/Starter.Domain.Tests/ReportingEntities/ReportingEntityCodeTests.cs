using Starter.Domain.Common;
using Starter.Domain.ReportingEntities;

namespace Starter.Domain.Tests.ReportingEntities;

public sealed class ReportingEntityCodeTests
{
    [Theory]
    [InlineData("ACME")]
    [InlineData("GLOBAL")]
    public void Constructor_ShouldCreateReportingEntityCode(string value)
    {
        var code = new ReportingEntityCode(value);

        Assert.Equal(value, code.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_WhenValueIsEmpty_ShouldThrowDomainException(
        string value)
    {
        void Action() => _ = new ReportingEntityCode(value);

        Assert.Throws<DomainException>(Action);
    }

    [Theory]
    [InlineData("  ACME  ", "ACME")]
    [InlineData(" GLOBAL ", "GLOBAL")]
    public void Constructor_ShouldTrimValue(string value, string expected)
    {
        var code = new ReportingEntityCode(value);

        Assert.Equal(expected, code.Value);
    }
}
