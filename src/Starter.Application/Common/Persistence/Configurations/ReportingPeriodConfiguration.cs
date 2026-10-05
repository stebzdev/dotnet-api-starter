using Starter.Domain.ReportingEntities;
using Starter.Domain.ReportingPeriods;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Starter.Application.Common.Persistence.Configurations;

public sealed class ReportingPeriodConfiguration : IEntityTypeConfiguration<ReportingPeriod>
{
    public void Configure(EntityTypeBuilder<ReportingPeriod> builder)
    {
        builder.ToTable("reporting_periods");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new ReportingPeriodId(value));

        builder.Property(x => x.ReportingEntityId)
            .HasConversion(
                id => id.Value,
                value => new ReportingEntityId(value));

        builder.ComplexProperty(x => x.ReferencePeriod,
            referencePeriod =>
            {
                referencePeriod.Property(x => x.Year)
                    .HasColumnName("ReferenceYear");

                referencePeriod.Property(x => x.Month)
                    .HasColumnName("ReferenceMonth");
            });

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.HasOne<ReportingEntity>()
            .WithMany()
            .HasForeignKey(x => x.ReportingEntityId)
            .OnDelete(DeleteBehavior.Restrict);

        // The unique index on ReportingEntityId, ReferenceYear and ReferenceMonth
        // is preserved in the database by the existing migration.
        // EF Core 10 cannot define an index over ReferencePeriod complex type properties.
    }
}
