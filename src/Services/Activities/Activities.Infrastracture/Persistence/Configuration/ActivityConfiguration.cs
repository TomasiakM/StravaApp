using Activities.Domain.Aggregates.Activities;
using Activities.Domain.Aggregates.Activities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Activities.Infrastracture.Persistence.Configuration;
internal sealed class ActivityConfiguration : IEntityTypeConfiguration<ActivityAggregate>
{
    public void Configure(EntityTypeBuilder<ActivityAggregate> builder)
    {
        builder.ToTable("Activities");

        builder.HasKey(t => t.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasConversion(
                e => e.Value,
                e => ActivityId.Create(e));

        builder.HasIndex(e => e.StravaId)
            .IsUnique();

        builder.HasIndex(e => e.StravaUserId);

        builder.OwnsOne(e => e.Speed, owned =>
        {
            owned.Property(p => p.MaxSpeed)
                .HasColumnName(nameof(Speed.MaxSpeed));

            owned.Property(p => p.AverageSpeed)
                .HasColumnName(nameof(Speed.AverageSpeed));
        });

        builder.OwnsOne(e => e.Time, owned =>
        {
            owned.Property(p => p.ElapsedTime)
                .HasColumnName(nameof(Time.ElapsedTime));

            owned.Property(p => p.MovingTime)
                .HasColumnName(nameof(Time.MovingTime));

            owned.Property(p => p.StartDate)
                .HasColumnName(nameof(Time.StartDate));

            owned.Property(p => p.StartDateLocal)
                .HasColumnName(nameof(Time.StartDateLocal));
        });

        builder.OwnsOne(e => e.Watts, owned =>
        {
            owned.Property(p => p.DeviceWatts)
                .HasColumnName(nameof(Watts.DeviceWatts));

            owned.Property(p => p.MaxWatts)
                .HasColumnName(nameof(Watts.MaxWatts));

            owned.Property(p => p.AverageWatts)
                .HasColumnName(nameof(Watts.AverageWatts));
        });

        builder.OwnsOne(e => e.Heartrate, owned =>
        {
            owned.Property(p => p.HasHeartrate)
                .HasColumnName(nameof(Heartrate.HasHeartrate));

            owned.Property(p => p.MaxHeartrate)
                .HasColumnName(nameof(Heartrate.MaxHeartrate));

            owned.Property(p => p.AverageHeartrate)
                .HasColumnName(nameof(Heartrate.AverageHeartrate));
        });

        builder.OwnsOne(e => e.Map, owned =>
        {
            owned.Property(e => e.Polyline)
                .HasColumnName(nameof(Map.Polyline));

            owned.Property(p => p.SummaryPolyline)
                .HasColumnName(nameof(Map.SummaryPolyline));

            owned.OwnsOne(p => p.StartLatlng, e =>
            {
                e.Property(p => p.Latitude)
                    .HasColumnName("StartLat");
                e.Property(p => p.Longitude)
                    .HasColumnName("StartLng");
            });

            owned.OwnsOne(p => p.EndLatlng, e =>
            {
                e.Property(p => p.Latitude)
                    .HasColumnName("EndLat");
                e.Property(p => p.Longitude)
                    .HasColumnName("EndLng");
            });
        });
    }
}
