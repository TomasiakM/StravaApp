using Athletes.Application.Features.Athletes.Commands.Create;
using Athletes.Application.Features.Athletes.Commands.Update;
using Athletes.Application.Interfaces;
using Common.MessageBroker.Contracts.Athletes;
using MassTransit;
using MediatR;

namespace Athletes.Application.Consumers;
public sealed class ReceivedAthleteDataEventConsumer : IConsumer<ReceivedAthleteDataEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISender _sender;

    public ReceivedAthleteDataEventConsumer(IUnitOfWork unitOfWork, ISender sender)
    {
        _unitOfWork = unitOfWork;
        _sender = sender;
    }

    public async Task Consume(ConsumeContext<ReceivedAthleteDataEvent> context)
    {
        var athleteExists = await _unitOfWork.Athletes.AnyAsync(e => e.StravaUserId == context.Message.Id);
        if (athleteExists)
        {
            await _sender.Send(new UpdateAthleteCommand(
                context.Message.Id,
                context.Message.Username,
                context.Message.Firstname,
                context.Message.Lastname,
                context.Message.CreatedAt,
                context.Message.Profile,
                context.Message.ProfileMedium));

        }
        else
        {
            await _sender.Send(new CreateAthleteCommand(
                context.Message.Id,
                context.Message.Username,
                context.Message.Firstname,
                context.Message.Lastname,
                context.Message.CreatedAt,
                context.Message.Profile,
                context.Message.ProfileMedium));
        }
    }
}
