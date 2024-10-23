using Common.Domain.Enums;
using Common.Domain.Models;

namespace Common.MessageBroker.Saga.ProcessActivityData.Events;
public record ActivityProcessedEvent(
    Guid CorrelationId,
    long StravaActivityId,
    long StravaUserId,
    DateTime CreatedAt,
    SportType SportType,
    List<LatLng> LatLngs);
