using Common.Domain.Models;

namespace Common.MessageBroker.Contracts.Activities;
public record ReceivedActivityTrackDetailsEvent(
    long StravaUserId,
    long StravaActivityId,
    List<LatLng> LatLngs);
