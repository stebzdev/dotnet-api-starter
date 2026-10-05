using Starter.Domain.Common;

namespace Starter.Domain.ReportingPeriods;

public readonly record struct ReferencePeriod
{
    public ReferencePeriod(int year, int month)
    {
        if (year <= 0)
        {
            throw new DomainException("Reference year must be greater than zero.");
        }

        if (month is < 1 or > 12)
        {
            throw new DomainException( "Reference month must be between 1 and 12.");
        }

        Year = year;
        Month = month;
    }

    public int Year { get; }
    public int Month { get; }
}
