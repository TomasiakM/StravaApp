using Auth.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Auth.Application.Features.Token.Commands.Delete;
internal sealed class DeleteTokenCommandHandler : IRequestHandler<DeleteTokenCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteTokenCommandHandler> _logger;

    public DeleteTokenCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteTokenCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteTokenCommand request, CancellationToken cancellationToken)
    {
        var token = await _unitOfWork.Tokens
            .GetAsync(e => e.StravaUserId == request.StravaUserId);

        if (token is not null)
        {
            _unitOfWork.Tokens.Delete(token);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User:{UserId} token has been removed successfully.", request.StravaUserId);

            return Unit.Value;
        }

        _logger.LogWarning("User:{UserId} token not found.", request.StravaUserId);

        return Unit.Value;
    }
}
