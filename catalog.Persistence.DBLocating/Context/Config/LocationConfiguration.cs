using catalog.Core.Locating.Domain.Location;
using catalog.Core.Locating.Domain.Region;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace catalog.Persistence.DBLocating.Context.Config;

public class LocationConfiguration : IEntityTypeConfiguration<LocationObject>
{
    public void Configure(EntityTypeBuilder<LocationObject> entity)
    {

        entity.ToTable("LocationObject");
        entity.HasComment("Таблица справочника ГАР");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasConversion(v => v.Value, v => new LocationObjectId(v))
            .ValueGeneratedNever();

        entity.Property(e => e.Uid)
            .IsRequired();
        entity.HasIndex(e => e.Uid);

        entity.Property(e => e.ParentId)
            .HasConversion(v => v.Value, v => new LocationObjectId(v))
            .IsRequired();
        entity.HasIndex(e => e.ParentId);

        entity.Property(e => e.LocationType)
            .HasConversion(v => v.Code, v => new LocationType(v))
            .IsRequired();

        entity.HasOne<Region>()
          .WithMany()
          .HasForeignKey(p => p.RegionId)
          .IsRequired()
          .OnDelete(DeleteBehavior.NoAction);

        entity.Property(e => e.ProperName)
            .HasMaxLength(300);
        entity.Property(e => e.Name)
            .HasMaxLength(350);

        entity.Property(e => e.PlainCode)
            .HasConversion(v => v.Value, v => LocationPlainCode.Create(v))
            .HasMaxLength(15)
            .IsRequired();
        entity.Property(e => e.PostCode)
            .HasConversion(v => v.Value, v => LocationPostCode.Create(v))
            .HasMaxLength(6)
            .IsRequired();
        entity.Property(e => e.Okato)
            .HasConversion(v => v.Value, v => LocationOkato.Create(v))
            .HasMaxLength(11)
            .IsRequired();

        entity
            .Property(e => e.Structure)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<IReadOnlyList<LocationObjectId>>(v, (JsonSerializerOptions)null),
                new ValueComparer<IReadOnlyList<LocationObjectId>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()))
            .HasMaxLength(1024);
    }
}
