using Starter.Domain.Common;
using Starter.Domain.ReportingPeriods;

namespace Starter.Domain.Tests.ReportingPeriods;

public sealed class ReferencePeriodTests
{
    [Fact]
    public void Create_WithValidYearAndMonth_ShouldCreateReferencePeriod()
    {
        var referencePeriod = new ReferencePeriod(2026, 9);

        Assert.Equal(2026, referencePeriod.Year);
        Assert.Equal(9, referencePeriod.Month);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_WithInvalidYear_ShouldThrowDomainException(int year)
    {
        void action()
        {
            var referencePeriod = new ReferencePeriod(year, 9);
        }

        Assert.Throws<DomainException>(action);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(13)]
    [InlineData(-1)]
    public void Create_WithInvalidMonth_ShouldThrowDomainException(int month)
    {
        void action()
        {
            var referencePeriod = new ReferencePeriod(2026, month);
        }

        Assert.Throws<DomainException>(action);
    }
}
