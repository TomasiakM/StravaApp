using Common.MessageBroker.Contracts.Athletes;
using MapsterMapper;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Strava.Application.Dtos.Athlete;
using Strava.Application.Dtos.Auth;
using Strava.Application.Interfaces;
using Strava.Contracts.Authorization;
using Strava.Domain.Aggregates.Token;
using Strava.Infrastructure.Interfaces;
using Strava.Infrastructure.Settings;
using System.Security.Claims;
using System.Text.Json;

namespace Strava.Infrastructure.Services;
internal class StravaAuthenticationService : IStravaAuthenticationService
{
    private readonly StravaSettings _stravaSettings;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IBus _bus;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public StravaAuthenticationService(IOptions<StravaSettings> stravaSettingsOptions, IMapper mapper, ITokenService tokenService, IUnitOfWork unitOfWork, IHttpClientFactory httpClientFactory, IBus bus, IHttpContextAccessor httpContextAccessor)
    {
        _stravaSettings = stravaSettingsOptions.Value;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _httpClientFactory = httpClientFactory;
        _bus = bus;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AuthResponse> LoginAsync(AuthRequest request, CancellationToken cancellationToken = default)
    {
        var authenticationResponse = await StravaAuthorizationRequestAsync<StravaAuthorizationResponse>(
            request.Code, isRefreshTokenRequest: false, cancellationToken);

        var token = await _unitOfWork.Tokens
            .FindAsync(e => e.StravaUserId == authenticationResponse.Athlete.Id, cancellationToken);
        if (token is not null)
        {
            token.Update(
                authenticationResponse.RefreshToken,
                authenticationResponse.AccessToken,
                authenticationResponse.ExpiresAt);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        if (token is null)
        {
            token = TokenAggregate.Create(
                authenticationResponse.Athlete.Id,
                authenticationResponse.RefreshToken,
                authenticationResponse.AccessToken,
                authenticationResponse.ExpiresAt);

            _unitOfWork.Tokens.Add(token);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _bus.Publish(new NewAthleteLoggedInEvent(authenticationResponse.Athlete.Id));
        }

        await _bus.Publish(_mapper.Map<ReceivedAthleteDataEvent>(authenticationResponse.Athlete));

        var accessToken = _tokenService.GenerateToken(token.StravaUserId);

        await AddRefreshCookieToContext(token.StravaUserId);

        return new AuthResponse(
            accessToken,
            _mapper.Map<AthleteSummitResponse>(authenticationResponse.Athlete));
    }

    public RefreshTokenResponse RefreshToken()
    {
        var claimUserId = _httpContextAccessor.HttpContext!.User.Claims.First(e => e.Type == ClaimTypes.NameIdentifier);
        var stravaUserId = long.Parse(claimUserId.Value);

        var accessToken = _tokenService.GenerateToken(stravaUserId);

        return new RefreshTokenResponse(accessToken);
    }

    public async Task<TokenAggregate?> GetUserToken(long stravaUserId, CancellationToken cancellationToken = default)
    {
        var token = await _unitOfWork.Tokens
            .FindAsync(e => e.StravaUserId == stravaUserId, cancellationToken);

        if (token is null)
        {
            return null;
        }

        var tokenExpiresAt = DateTimeOffset.FromUnixTimeSeconds(token.ExpiresAt);
        if (DateTime.UtcNow.AddMinutes(30) > tokenExpiresAt)
        {
            var refreshResponse = await StravaAuthorizationRequestAsync<StravaRefreshTokenResponse>(token.RefreshToken, isRefreshTokenRequest: true, cancellationToken);

            token.Update(
                refreshResponse.RefreshToken,
                refreshResponse.AccessToken,
                refreshResponse.ExpiresAt);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return token;
    }

    private async Task AddRefreshCookieToContext(long stravaUserId)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, stravaUserId.ToString()),
        };

        var claimsIdentity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        await _httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
    }

    private async Task<TResponse> StravaAuthorizationRequestAsync<TResponse>(string token, bool isRefreshTokenRequest = false, CancellationToken cancellationToken = default)
    {
        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new("https://www.strava.com/api/v3");

        var body = new List<KeyValuePair<string, string>>
        {
            new("client_id", _stravaSettings.ClientId.ToString()),
            new("client_secret", _stravaSettings.ClientSecret),
        };

        if (isRefreshTokenRequest)
        {
            body.Add(new("refresh_token", token));
            body.Add(new("grand_type", "refresh_token"));
        }
        else
        {
            body.Add(new("code", token));
            body.Add(new("grand_type", "authorization_code"));
        }

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/oauth/token")
        {
            Content = new FormUrlEncodedContent(body)
        };

        var res = await client.SendAsync(requestMessage, cancellationToken);

        res.EnsureSuccessStatusCode();

        var contentStream = await res.Content.ReadAsStreamAsync(cancellationToken);
        var deserializedData = JsonSerializer.Deserialize<TResponse>(contentStream, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower });

        if (deserializedData is null)
        {
            throw new Exception("Błąd podczas serializacji");
        }

        return deserializedData;
    }
}
