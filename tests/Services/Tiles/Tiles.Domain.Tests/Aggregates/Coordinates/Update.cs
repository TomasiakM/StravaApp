using Common.Domain.Models;
using Tiles.Domain.Aggregates.Coordinates;

namespace Tiles.Domain.Tests.Aggregates.Coordinates;
public class Update
{
    [Fact]
    public void ShouldUpdateCoordinatesAggregate()
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

        var coordinates2 = new List<LatLng>()
        {
            LatLng.Create(2.54, 22.3245),
            LatLng.Create(2.573, 22.452),
            LatLng.Create(2.5, 21.452),
            LatLng.Create(2.73, 21.52),
            LatLng.Create(2.3, 20.452),
        };

        coordinatesAggregate.Update(coordinates2);


        Assert.Equal(stravaActivityId, coordinatesAggregate.StravaActivityId);
        Assert.True(coordinates2.SequenceEqual(coordinatesAggregate.Coordinates));
    }
}
