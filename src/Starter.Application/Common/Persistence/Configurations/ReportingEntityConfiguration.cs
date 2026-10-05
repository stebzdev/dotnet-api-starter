using Starter.Domain.ReportingEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Starter.Application.Common.Persistence.Configurations;

internal sealed class ReportingEntityConfiguration : IEntityTypeConfiguration<ReportingEntity>
{
    public void Configure(EntityTypeBuilder<ReportingEntity> builder)
    {
        builder.ToTable("reporting_entities");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new ReportingEntityId(value));

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Code)
            .HasConversion(
                code => code.Value,
                value => new ReportingEntityCode(value))
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.Code)
            .IsUnique();
    }
}
