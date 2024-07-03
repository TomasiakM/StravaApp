using Auth.Application.Interfaces;
using Common.MessageBroker.Contracts.Athletes;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Auth.Application.Consumers;
public sealed class UnauthorizeAthleteEventConsumer : IConsumer<UnauthorizeAthleteEvent>
{
    private readonly ILogger<UnauthorizeAthleteEventConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UnauthorizeAthleteEventConsumer(ILogger<UnauthorizeAthleteEventConsumer> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<UnauthorizeAthleteEvent> context)
    {
        _logger.LogInformation("Deleting user:{UserId} token.", context.Message.StravaUserId);

        var token = await _unitOfWork.Tokens
            .GetAsync(e => e.StravaUserId == context.Message.StravaUserId);

        if (token is not null)
        {
            _unitOfWork.Tokens.Delete(token);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("User:{UserId} token has been removed successfully.", context.Message.StravaUserId);

            return;
        }

        _logger.LogWarning("User:{UserId} token not found.", context.Message.StravaUserId);
    }
}
