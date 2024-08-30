using Activities.Domain.Aggregates.Activities;
using Activities.Domain.Aggregates.Activities.ValueObjects;
using Activities.Domain.Aggregates.Streams;
using Activities.Domain.Aggregates.Streams.ValueObjects;
using Common.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Activities.Infrastracture.Persistence.Configuration;
internal sealed class StreamConfiguration : IEntityTypeConfiguration<StreamAggregate>
{
    public void Configure(EntityTypeBuilder<StreamAggregate> builder)
    {
        builder.ToTable("Streams");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasConversion(
                e => e.Value,
                e => StreamId.Create(e));

        builder.Property(e => e.ActivityId)
            .HasConversion(
                e => e.Value,
                e => ActivityId.Create(e));
        builder.HasOne<ActivityAggregate>()
            .WithOne()
            .HasForeignKey<StreamAggregate>(e => e.ActivityId);

        builder.Property(e => e.Heartrate)
            .HasConversion(
                e => JsonSerializer.Serialize(e, (JsonSerializerOptions?)null),
                e => JsonSerializer.Deserialize<List<int>>(e, (JsonSerializerOptions?)null)!)
            .HasField("_heartrate")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property(e => e.Cadence)
            .HasConversion(
                e => JsonSerializer.Serialize(e, (JsonSerializerOptions?)null),
                e => JsonSerializer.Deserialize<List<int>>(e, (JsonSerializerOptions?)null)!)
            .HasField("_cadence")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property(e => e.Distance)
            .HasConversion(
                e => JsonSerializer.Serialize(e, (JsonSerializerOptions?)null),
                e => JsonSerializer.Deserialize<List<float>>(e, (JsonSerializerOptions?)null)!)
            .HasField("_distance")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property(e => e.Altitude)
            .HasConversion(
                e => JsonSerializer.Serialize(e, (JsonSerializerOptions?)null),
                e => JsonSerializer.Deserialize<List<float>>(e, (JsonSerializerOptions?)null)!)
            .HasField("_altitude")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property(e => e.LatLngs)
            .HasConversion(
                e => JsonSerializer.Serialize(e, (JsonSerializerOptions?)null),
                e => JsonSerializer.Deserialize<List<LatLng>>(e, (JsonSerializerOptions?)null) ?? new List<LatLng>())
            .HasField("_latLngs")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
