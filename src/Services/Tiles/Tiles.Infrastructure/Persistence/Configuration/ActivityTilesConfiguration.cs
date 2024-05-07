using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tiles.Domain.Aggregates.ActivityTiles;
using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;

namespace Tiles.Infrastructure.Persistence.Configuration;
internal sealed class ActivityTilesConfiguration : IEntityTypeConfiguration<ActivityTilesAggregate>
{
    public void Configure(EntityTypeBuilder<ActivityTilesAggregate> builder)
    {
        builder.ToTable("ActivityTiles");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasConversion(
                e => e.Value,
                e => ActivityTilesId.Create(e));

        builder.HasIndex(e => e.StravaActivityId)
            .IsUnique();

        builder.OwnsMany(e => e.Tiles, e =>
        {
            e.ToTable("Tiles");

            e.WithOwner()
                .HasForeignKey(nameof(ActivityTilesId));

            e.HasKey(nameof(ActivityTilesId), "X", "Y", "Z");
        });

        builder.Metadata.FindNavigation(nameof(ActivityTilesAggregate.Tiles))!.
            SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(e => e.NewTiles, e =>
        {
            e.ToTable("NewTiles");

            e.WithOwner()
                .HasForeignKey(nameof(ActivityTilesId));

            e.HasKey(nameof(ActivityTilesId), "X", "Y", "Z");
        });

        builder.Metadata.FindNavigation(nameof(ActivityTilesAggregate.NewTiles))!.
            SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(e => e.NewClusterTiles, e =>
        {
            e.ToTable("NewClusterTiles");

            e.WithOwner()
                .HasForeignKey(nameof(ActivityTilesId));

            e.HasKey(nameof(ActivityTilesId), "X", "Y", "Z");
        });

        builder.Metadata.FindNavigation(nameof(ActivityTilesAggregate.NewClusterTiles))!.
            SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(e => e.NewSquareTiles, e =>
        {
            e.ToTable("NewSquareTiles");

            e.WithOwner()
                .HasForeignKey(nameof(ActivityTilesId));

            e.HasKey(nameof(ActivityTilesId), "X", "Y", "Z");
        });

        builder.Metadata.FindNavigation(nameof(ActivityTilesAggregate.NewSquareTiles))!.
            SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
