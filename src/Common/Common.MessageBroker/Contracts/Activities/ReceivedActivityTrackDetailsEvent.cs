using Common.Domain.Models;

namespace Common.MessageBroker.Contracts.Activities;
public record ReceivedActivityTrackDetailsEvent(
    long StravaUserId,
    long StravaActivityId,
    DateTime CreatedAt,
    List<LatLng> LatLngs);
