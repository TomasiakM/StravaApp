using Athletes.Domain.Aggregates.Athletes;
using Athletes.Domain.Aggregates.Athletes.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Athletes.Infrastructure.Persistence.Configuration;
internal class AthleteConfiguration
    : IEntityTypeConfiguration<AthleteAggregate>
{
    public void Configure(EntityTypeBuilder<AthleteAggregate> builder)
    {
        builder.ToTable("Athletes");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasConversion(
                e => e.Value,
                e => AthleteId.Create(e));

        builder.HasIndex(e => e.StravaUserId);
    }
}
