using MediatR;

namespace Athletes.Application.Features.Athletes.Commands.Delete;
public record DeleteAthleteCommand(
    long StravaUserId) : IRequest<Unit>;
