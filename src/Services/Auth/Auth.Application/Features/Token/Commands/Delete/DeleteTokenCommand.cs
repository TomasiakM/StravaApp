using MediatR;

namespace Auth.Application.Features.Token.Commands.Delete;
public record DeleteTokenCommand(
    long StravaUserId) : IRequest<Unit>;
