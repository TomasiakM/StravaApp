using Common.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;
using Tiles.Domain.Aggregates.Coordinates;
using Tiles.Domain.Aggregates.Coordinates.ValueObjects;
using Tiles.Infrastructure.Persistence.Configuration.Utils;

namespace Tiles.Infrastructure.Persistence.Configuration;
internal sealed class CoordinatesConfiguration : IEntityTypeConfiguration<CoordinatesAggregate>
{
    public void Configure(EntityTypeBuilder<CoordinatesAggregate> builder)
    {
        builder.ToTable("Coordinates");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasConversion(
                e => e.Value,
                e => CoordinatesId.Create(e));

        builder.HasIndex(e => e.StravaActivityId)
            .IsUnique();

        builder.Metadata.FindNavigation(nameof(CoordinatesAggregate.Coordinates))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.Property(e => e.Coordinates)
            .HasConversion(
                e => JsonSerializer.Serialize(e, (JsonSerializerOptions?)null),
                e => JsonSerializer.Deserialize<List<LatLng>>(e, (JsonSerializerOptions?)null)!,
                new LatLngModelComparer());
    }
}
