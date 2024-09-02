using Common.Domain.Models;

namespace Common.MessageBroker.Saga.ProcessActivityData.Messages;
public record ProcessTilesMessage(
    Guid CorrelationId,
    long StravaActivityId,
    long StravaUserId,
    DateTime CreatedAt,
    List<LatLng> LatLngs);