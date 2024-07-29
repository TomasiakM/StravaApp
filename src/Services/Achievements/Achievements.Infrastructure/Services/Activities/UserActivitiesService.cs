using Achievements.Application.Interfaces.Services;
using Common.Domain.Models;
using Common.MessageBroker.Contracts.Activities.GetUserActivities;
using MassTransit;

namespace Achievements.Infrastructure.Services.Activities;
internal sealed class UserActivitiesService : IUserActivitiesService
{
    private readonly IRequestClient<GetUserActivitiesRequest> _client;

    public UserActivitiesService(IRequestClient<GetUserActivitiesRequest> client)
    {
        _client = client;
    }

    public async Task<IEnumerable<Activity>> GetAllAsync(long stravaUserId)
    {
        var response = await _client.GetResponse<GetUserActivitiesResponse>(stravaUserId);

        return response.Message.Activities;
    }
}
