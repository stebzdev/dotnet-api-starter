using Microsoft.EntityFrameworkCore;
using Starter.Domain.SampleItems;

namespace Starter.Application.Common.Persistence;

public sealed class StarterDbContext(DbContextOptions<StarterDbContext> options) : DbContext(options)
{
    public DbSet<SampleItem> SampleItems => Set<SampleItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StarterDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
