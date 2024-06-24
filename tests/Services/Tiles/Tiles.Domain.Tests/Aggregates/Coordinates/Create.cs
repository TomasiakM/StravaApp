using Common.Domain.Models;
using Tiles.Domain.Aggregates.Coordinates;

namespace Tiles.Domain.Tests.Aggregates.Coordinates;
public class Create
{
    [Fact]
    public void ShouldCreateCoordinatesAggregate()
    {
        var stravaActivityId = 4;
        var coordinates = new List<LatLng>()
        {
            LatLng.Create(2.54, 22.3245),
            LatLng.Create(2.573, 22.452),
        };

        var coordinatesAggregate = CoordinatesAggregate.Create(
            stravaActivityId,
            coordinates);

        Assert.Equal(stravaActivityId, coordinatesAggregate.StravaActivityId);
        Assert.True(coordinates.SequenceEqual(coordinatesAggregate.Coordinates));
    }
}
