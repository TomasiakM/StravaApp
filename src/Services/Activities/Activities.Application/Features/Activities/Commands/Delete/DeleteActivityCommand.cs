using MediatR;

namespace Activities.Application.Features.Activities.Commands.Delete;
public record DeleteActivityCommand(
    long StravaActivityId) : IRequest<Unit>;
