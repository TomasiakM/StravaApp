using Athletes.Application.Interfaces;
using Athletes.Domain.Aggregates.Athletes;
using Common.MessageBroker.Contracts.Athletes;
using MassTransit;

namespace Athletes.Application.Consumers;
public sealed class ReceivedAthleteDataEventConsumer : IConsumer<ReceivedAthleteDataEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public ReceivedAthleteDataEventConsumer(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<ReceivedAthleteDataEvent> context)
    {
        var athlete = await _unitOfWork.Athletes.FindAsync(e => e.StravaUserId == context.Message.Id);

        var message = context.Message;
        if (athlete is null)
        {
            athlete = AthleteAggregate.Create(
                message.Id,
                message.Username,
                message.Firstname,
                message.Lastname,
                message.Profile,
                message.ProfileMedium,
                message.CreatedAt);

            _unitOfWork.Athletes.Add(athlete);
            await _unitOfWork.SaveChangesAsync();

            return;
        }

        athlete.Update(
            message.Username,
            message.Firstname,
            message.Lastname,
            message.Profile,
            message.ProfileMedium);

        await _unitOfWork.SaveChangesAsync();
    }
}
