using Auth.Domain.Aggregates.Token;
using Auth.Domain.Aggregates.Token.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Persistence.Configuration;
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
