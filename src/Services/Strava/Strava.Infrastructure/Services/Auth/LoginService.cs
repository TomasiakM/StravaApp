using Common.MessageBroker.Contracts.Athletes;
using MapsterMapper;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Strava.Application.Dtos.Athlete;
using Strava.Application.Dtos.Auth;
using Strava.Application.Interfaces;
using Strava.Application.Interfaces.Services.Auth;
using Strava.Domain.Aggregates.Token;
using Strava.Infrastructure.Interfaces;
using System.Security.Claims;

namespace Strava.Infrastructure.Services.Auth;
internal sealed class LoginService : ILoginService
{
    private readonly IConfirmStravaAuthenticationCodeService _confirmStravaAuthenticationCodeService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBus _bus;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoginService(IConfirmStravaAuthenticationCodeService confirmStravaAuthenticationCodeService, IUnitOfWork unitOfWork, IBus bus, IMapper mapper, ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
    {
        _confirmStravaAuthenticationCodeService = confirmStravaAuthenticationCodeService;
        _unitOfWork = unitOfWork;
        _bus = bus;
        _mapper = mapper;
        _tokenService = tokenService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AuthResponse> LoginAsync(AuthRequest request, CancellationToken cancellationToken = default)
    {
        var authResponse = await _confirmStravaAuthenticationCodeService
            .AuthorizeAsync(request.Code, cancellationToken);

        var token = await _unitOfWork.Tokens.GetAsync(
            filter: e => e.StravaUserId == authResponse.Athlete.Id,
            cancellationToken: cancellationToken);

        if (token is not null)
        {
            token.Update(
                authResponse.RefreshToken,
                authResponse.AccessToken,
                authResponse.ExpiresAt);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        if (token is null)
        {
            token = TokenAggregate.Create(
                authResponse.Athlete.Id,
                authResponse.RefreshToken,
                authResponse.AccessToken,
                authResponse.ExpiresAt);

            _unitOfWork.Tokens.Add(token);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _bus.Publish(new NewAthleteLoggedInEvent(authResponse.Athlete.Id));
        }

        await _bus.Publish(_mapper.Map<ReceivedAthleteDataEvent>(authResponse.Athlete));

        var accessToken = _tokenService.GenerateToken(token.StravaUserId);

        await AddRefreshCookieToContext(token.StravaUserId);
        return new AuthResponse(
            accessToken,
            _mapper.Map<AthleteSummitResponse>(authResponse.Athlete));
    }

    private async Task AddRefreshCookieToContext(long stravaUserId)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, stravaUserId.ToString()),
        };

        var claimsIdentity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        await _httpContextAccessor.HttpContext!
            .SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
    }
}
