using MediatR;

namespace Strava.Application.Features.StravaHook.HandleEvent;
public record HandleEventCommand(
    ObjectTypeCommand ObjectType,
    long ObjectId,
    AspectTypeCommand AspectType,
    long OwnerId,
    int SubscriptionId,
    long EventTime,
    UpdateCommand Updates) : IRequest<Unit>;

public enum ObjectTypeCommand
{
    Activity,
    Athlete
}

public enum AspectTypeCommand
{
    Create,
    Update,
    Delete
}

public record UpdateCommand(
    string? Title,
    string? Type,
    string? Private,
    string? Authorized);
