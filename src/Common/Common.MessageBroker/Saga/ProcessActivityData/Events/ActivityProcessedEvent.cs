using Common.Domain.Models;

namespace Common.MessageBroker.Saga.ProcessActivityData.Events;
public record ActivityProcessedEvent(
    Guid CorrelationId,
    long StravaActivityId,
    long StravaUserId,
    DateTime CreatedAt,
    List<LatLng> LatLngs);
