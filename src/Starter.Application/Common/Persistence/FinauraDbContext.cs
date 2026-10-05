using Starter.Domain.ReportingEntities;
using Starter.Domain.ReportingPeriods;
using Microsoft.EntityFrameworkCore;

namespace Starter.Application.Common.Persistence;

public sealed class StarterDbContext(DbContextOptions<StarterDbContext> options) : DbContext(options)
{
    public DbSet<ReportingPeriod> ReportingPeriods => Set<ReportingPeriod>();
    public DbSet<ReportingEntity> ReportingEntities => Set<ReportingEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StarterDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
