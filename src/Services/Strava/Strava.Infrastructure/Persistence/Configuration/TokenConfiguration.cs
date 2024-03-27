using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Strava.Domain.Aggregates.Token;
using Strava.Domain.Aggregates.Token.ValueObjects;

namespace Strava.Infrastructure.Persistence.Configuration;
internal sealed class TokenConfiguration
    : IEntityTypeConfiguration<TokenAggregate>
{
    public void Configure(EntityTypeBuilder<TokenAggregate> builder)
    {
        builder.ToTable("Tokens");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasConversion(
                e => e.Value,
                e => TokenId.Create(e));

        builder.HasIndex(e => e.StravaUserId);
    }
}
