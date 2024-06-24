using Tiles.Domain.Aggregates.Coordinates.ValueObjects;

namespace Tiles.Domain.Tests.Aggregates.Coordinates.ValueObjects.CoordinatesIds;
public class Create
{
    [Fact]
    public void ShouldCreateCoordinatesId()
    {
        var coordinatesId = CoordinatesId.Create();

        Assert.NotEqual(Guid.Empty, coordinatesId.Value);
    }

    [Fact]
    public void ShouldCreateCoordinatesId2()
    {
        var guid = Guid.NewGuid();
        var coordinatesId = CoordinatesId.Create(guid);

        Assert.Equal(guid, coordinatesId.Value);
    }

    [Fact]
    public void Should2CoordinateIdsBeDifferents()
    {
        var coordinatesId = CoordinatesId.Create();
        var coordinatesId2 = CoordinatesId.Create();

        Assert.NotEqual(coordinatesId, coordinatesId2);
    }
}
