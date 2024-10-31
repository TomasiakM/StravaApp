using MediatR;

namespace Activities.Application.Features.Activities.Commands.DeleteAllUserActivities;
public record DeleteAllUserActivitiesCommand(
    long StravaUserId) : IRequest<Unit>;
