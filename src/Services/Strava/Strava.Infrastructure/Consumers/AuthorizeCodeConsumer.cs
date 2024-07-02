using Common.MessageBroker.Contracts.Authorization.AuthorizeUserCode;
using MapsterMapper;
using MassTransit;
using Strava.Infrastructure.Interfaces;

namespace Strava.Infrastructure.Consumers;
internal sealed class AuthorizeCodeConsumer
    : IConsumer<AuthorizeCodeRequest>
{
    private readonly IConfirmStravaAuthenticationCodeService _confirmStravaAuthenticationCodeService;
    private readonly IMapper _mapper;
    public AuthorizeCodeConsumer(IConfirmStravaAuthenticationCodeService confirmStravaAuthenticationCodeService, IMapper mapper)
    {
        _confirmStravaAuthenticationCodeService = confirmStravaAuthenticationCodeService;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<AuthorizeCodeRequest> context)
    {
        var userData = await _confirmStravaAuthenticationCodeService.AuthorizeAsync(context.Message.Code);

        var response = _mapper.Map<AuthorizeCodeResponse>(userData);

        await context.RespondAsync(response);
    }
}
