using Auth.Domain.Aggregates.Token.ValueObjects;

namespace Auth.Domain.Tests.Aggregates.Token.ValueObjects.TokenIds;
public class Create
{
    [Fact]
    public void ShouldCreateValidTokenId()
    {
        var tokenId = TokenId.Create();

        Assert.NotEqual(Guid.Empty, tokenId.Value);
    }

    [Fact]
    public void ShouldCreateValidTokenId2()
    {
        var guid = Guid.NewGuid();
        var tokenId = TokenId.Create(guid);

        Assert.Equal(guid, tokenId.Value);
    }

    [Fact]
    public void ShouldCreateDifferentAthletteIds()
    {
        var tokenId1 = TokenId.Create();
        var tokenId2 = TokenId.Create();

        Assert.False(tokenId1 == tokenId2);
    }
}
