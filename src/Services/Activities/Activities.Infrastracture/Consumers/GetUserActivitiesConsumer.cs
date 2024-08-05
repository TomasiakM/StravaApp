using Activities.Application.Interfaces;
using Common.Domain.Models;
using Common.MessageBroker.Contracts.Activities.GetUserActivities;
using MapsterMapper;
using MassTransit;

namespace Activities.Infrastracture.Consumers;
internal sealed class GetUserActivitiesConsumer : IConsumer<GetUserActivitiesRequest>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserActivitiesConsumer(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<GetUserActivitiesRequest> context)
    {
        var activities = await _unitOfWork.Activities
            .GetAllAsync(e => e.StravaUserId == context.Message.StravaUserId);
        var activityDtos = _mapper.Map<IEnumerable<Activity>>(activities);

        await context.RespondAsync(new GetUserActivitiesResponse(activityDtos));
    }
}
