using catalog.Core.Locating.Domain.Location;
using catalog.Core.Locating.Domain.Region;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace catalog.Persistence.DBLocating.Context.Config;

public class RegionConfiguration : IEntityTypeConfiguration<Region>
{
    public void Configure(EntityTypeBuilder<Region> entity)
    {

        entity.ToTable("Region");
        entity.HasComment("Регионы справочника ГАР");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasConversion(v => v.Value, v => new RegionId(v))
            .ValueGeneratedNever();

        entity.HasMany<LocationObject>()
              .WithOne()
              .HasForeignKey(p => p.RegionId)
              .OnDelete(DeleteBehavior.Cascade);
        entity.Ignore(e => e.Objects);

        entity.Property(e => e.RegionCode)
            .HasConversion(v => v.Value, v => new RegionCode(v))
            .HasMaxLength(2)
            .IsRequired();
        entity.Property(e => e.Version)
            .HasConversion(v => v.UpdateDate, v => new RegionVersion(v))
            .IsRequired();
    }
}
