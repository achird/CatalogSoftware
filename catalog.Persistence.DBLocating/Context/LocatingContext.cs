using catalog.Core.Locating.Application.Persistence.Repository;
using catalog.Core.Locating.Domain.Location;
using catalog.Core.Locating.Domain.Region;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

#nullable disable

namespace catalog.Persistence.DBLocating.Context;

public partial class LocatingContext : DbContext, IUnitOfWork
{
    public LocatingContext(DbContextOptions<LocatingContext> options)
        : base(options)
    {
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await SaveChangesAsync(cancellationToken);
    }

    public virtual DbSet<Region> Regions { get; set; }
    public virtual DbSet<LocationObject> LocationObjects { get; set; }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        //Pre - convention model configuration goes here
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.HasSequence<int>("RegionHiLo", "dbo")
            .StartsAt(1)
            .IncrementsBy(1);
    }
}
