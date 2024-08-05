using Achievements.Domain.Aggregates.Achievement;
using Achievements.Domain.Aggregates.Achievement.AchevementTypes.DistanceAchievements;
using Achievements.Domain.Aggregates.Achievement.Enums;
using Achievements.Domain.Aggregates.Achievement.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Achievements.Infrastructure.Persistence.Configurations;
public sealed class AchievementConfiguration : IEntityTypeConfiguration<Achievement>
{
    public void Configure(EntityTypeBuilder<Achievement> builder)
    {
        builder.ToTable("Achievements");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasConversion(
                e => e.Value,
                e => AchievementId.Create(e));

        builder.HasIndex(e => new { e.StravaUserId, e.AchievementType })
            .IsUnique();

        builder.HasDiscriminator(e => e.AchievementType)
            .HasValue<CumulativeDistanceAchievement>(AchievementType.CumulativeDistance)
            .HasValue<MaxDistanceAchievement>(AchievementType.MaxDistance)
            .HasValue<MonthlyCumulativeDistanceAchievement>(AchievementType.MonthlyCumulativeDistance)
            .HasValue<YearlyCumulativeDistanceAchievement>(AchievementType.YearlyCumulativeDistance);


        builder.Metadata.FindNavigation(nameof(Achievement.AchievementLevels))!.
            SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(e => e.AchievementLevels, alBuilder =>
        {
            alBuilder.WithOwner();

            alBuilder.ToTable("AchievementLevels");

            alBuilder.HasKey(e => e.Id);
        });
    }
}
