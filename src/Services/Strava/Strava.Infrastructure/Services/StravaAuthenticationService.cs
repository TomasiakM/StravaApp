using MapsterMapper;
using Microsoft.Extensions.Options;
using Strava.Application.Dtos.Athlete;
using Strava.Application.Dtos.Auth;
using Strava.Application.Interfaces;
using Strava.Contracts.Authorization;
using Strava.Domain.Aggregates.Token;
using Strava.Infrastructure.Interfaces;
using Strava.Infrastructure.Settings;
using System.Text.Json;

namespace Strava.Infrastructure.Services;
internal class StravaAuthenticationService : IStravaAuthenticationService
{
    private readonly StravaSettings _stravaSettings;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IHttpClientFactory _httpClientFactory;

    public StravaAuthenticationService(IOptions<StravaSettings> stravaSettingsOptions, IMapper mapper, ITokenService tokenService, IUnitOfWork unitOfWork, IHttpClientFactory httpClientFactory)
    {
        _stravaSettings = stravaSettingsOptions.Value;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _httpClientFactory = httpClientFactory;
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
                authenticationResponse.ExpiresIn);

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

            //TODO: Publish message with new athlete
        }


        var accessToken = _tokenService.GenerateToken(token.StravaUserId);


        return new AuthResponse(
            accessToken,
            _mapper.Map<AthleteSummitResponse>(authenticationResponse.Athlete));
    }


    public async Task<TokenAggregate?> RefreshToken(long stravaUserId, CancellationToken cancellationToken = default)
    {
        var token = await _unitOfWork.Tokens
            .FindAsync(e => e.StravaUserId == stravaUserId, cancellationToken);

        if (token is null)
        {
            return null;
        }

        var refreshResponse = await StravaAuthorizationRequestAsync<StravaRefreshTokenResponse>(token.RefreshToken, isRefreshTokenRequest: true, cancellationToken);

        token.Update(
            refreshResponse.RefreshToken,
            refreshResponse.AccessToken,
            refreshResponse.ExpiresIn);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return token;
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
            throw new Exception("Błąd podczas logowania");
        }

        return deserializedData;
    }
}
