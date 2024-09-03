using Common.Domain.Extensions;
using Common.MessageBroker.Saga.ProcessActivityData.Messages;
using Tiles.Application.Extensions;
using Tiles.Domain.Aggregates.ActivityTiles;
using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;

namespace Tiles.Application.Utils.ReceivedActivityTrackDetailsEventUtils;
public sealed class ExistingActivityTilesHandler
{
    public void UpdateAggregates(ProcessTilesMessage message, IEnumerable<ActivityTilesAggregate> activityTilesList)
    {
        var prevTiles = new HashSet<Tile>();
        var activityUpdated = false;

        foreach (var actTiles in activityTilesList)
        {
            if (actTiles.StravaActivityId == message.StravaActivityId)
            {
                var tiles = message.LatLngs.ToUniqueTiles(Tile.DEFAULT_TILE_ZOOM);
                actTiles.Update(prevTiles, tiles);
                prevTiles.AddRange(actTiles.Tiles);

                activityUpdated = true;

                continue;
            }

            if (!activityUpdated)
            {
                prevTiles.AddRange(actTiles.Tiles);
                continue;
            }

            actTiles.Update(prevTiles, actTiles.Tiles);
            prevTiles.AddRange(actTiles.Tiles);
        }
    }
}
