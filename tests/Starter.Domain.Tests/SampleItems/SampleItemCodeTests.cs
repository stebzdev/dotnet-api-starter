using Starter.Domain.Common;
using Starter.Domain.SampleItems;

namespace Starter.Domain.Tests.SampleItems;

public sealed class SampleItemCodeTests
{
    [Theory]
    [InlineData("ACME")]
    [InlineData("GLOBAL")]
    public void Constructor_ShouldCreateSampleItemCode(string value)
    {
        var code = new SampleItemCode(value);

        Assert.Equal(value, code.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_WhenValueIsEmpty_ShouldThrowDomainException(
        string value)
    {
        void Action() => _ = new SampleItemCode(value);

        Assert.Throws<DomainException>(Action);
    }

    [Theory]
    [InlineData("  ACME  ", "ACME")]
    [InlineData(" GLOBAL ", "GLOBAL")]
    public void Constructor_ShouldTrimValue(string value, string expected)
    {
        var code = new SampleItemCode(value);

        Assert.Equal(expected, code.Value);
    }
}
