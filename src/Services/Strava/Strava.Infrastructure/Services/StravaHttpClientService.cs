using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using Strava.Application.Interfaces;
using Strava.Infrastructure.Settings;
using System.Net;
using System.Text.Json;

namespace Strava.Infrastructure.Services;
internal sealed class StravaHttpClientService
{
    private readonly ILogger<StravaHttpClientService> _logger;
    private readonly IStravaAuthenticationService _stravaAuthenticationService;
    private readonly HttpClient _httpClient;

    public StravaHttpClientService(ILogger<StravaHttpClientService> logger, IStravaAuthenticationService stravaAuthenticationService, IHttpClientFactory httpClientFactory, IOptions<StravaSettings> stravaSettings)
    {
        _logger = logger;
        _stravaAuthenticationService = stravaAuthenticationService;
        _httpClient = httpClientFactory.CreateClient();

        _httpClient.BaseAddress = new Uri(stravaSettings.Value.BaseUrl);
    }

    public async Task<TResponse> GetResponse<TResponse>(long stravaUserId, string url, IDictionary<string, string>? queryDictionary = null, CancellationToken cancellationToken = default)
    {
        var response = await UnauthorizedPolicy(stravaUserId)
            .WrapAsync(RateLimitPolicy())
            .WrapAsync(ServerErrorRetryPolicy())
            .ExecuteAsync(async () =>
            {
                var requestMessage = await CreateRequestMessage(stravaUserId, url, queryDictionary);

                return await _httpClient.SendAsync(requestMessage);
            });

        response.EnsureSuccessStatusCode();

        var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var deserializedData = JsonSerializer.Deserialize<TResponse>(contentStream, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower });

        if (deserializedData is null)
        {
            throw new Exception("Invalid deserialization data");
        }

        return deserializedData;
    }

    private async Task<HttpRequestMessage> CreateRequestMessage(long stravaUserId, string url, IDictionary<string, string>? queryDictionary = null)
    {
        if (queryDictionary?.Count > 0)
        {
            var encodedContent = new FormUrlEncodedContent(queryDictionary);
            var query = await encodedContent.ReadAsStringAsync();

            url += $"?{query}";
        }

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

        var token = await _stravaAuthenticationService.GetUserToken(stravaUserId);
        requestMessage.Headers.Authorization = new("Bearer", token?.AccessToken);

        return requestMessage;
    }

    private AsyncRetryPolicy<HttpResponseMessage> RateLimitPolicy()
    {
        return Policy
            .HandleResult<HttpResponseMessage>(res => res.StatusCode == HttpStatusCode.TooManyRequests)
            .WaitAndRetryAsync(5, (retrycount) =>
            {
                if (retrycount % 2 == 1)
                {
                    var wait = TimeSpan.FromMinutes(15 - (DateTime.UtcNow.Minute % 15));
                    _logger.LogInformation("Waiting for {Wait}. Request will be retried for {Retrycount} time. Next retry fetch at {Time}.", wait, retrycount, DateTime.UtcNow.Add(wait));

                    return wait;
                }

                var waitTime = DateTime.Today.AddDays(1) - DateTime.UtcNow;
                _logger.LogInformation("Waiting for {Wait}. Request will be retried for {Retrycount} time. Next retry fetch at {Time}.", waitTime, retrycount, DateTime.UtcNow.Add(waitTime));

                return waitTime.Add(TimeSpan.FromMinutes(1));
            });
    }

    private AsyncRetryPolicy<HttpResponseMessage> ServerErrorRetryPolicy()
    {
        return Policy
            .HandleResult<HttpResponseMessage>(res => (int)res.StatusCode >= 500)
            .WaitAndRetryAsync(8, (retrycount) =>
            {
                var waitTime = TimeSpan.FromSeconds(Math.Pow(2, retrycount));
                _logger.LogInformation("Api error occured with status code >= 500. Waiting for {Wait}. Request will be retried for {Retrycount} time.", waitTime, retrycount);

                return waitTime;
            });
    }

    private AsyncRetryPolicy<HttpResponseMessage> UnauthorizedPolicy(long userStravaId)
    {
        return Policy
            .HandleResult<HttpResponseMessage>(res => res.StatusCode == HttpStatusCode.Unauthorized)
            .RetryAsync(1, async (response, retrycount, context) =>
            {
                _logger.LogInformation("Response unauthorized, getting token for {UserId} and retrying request.", userStravaId);

                var token = await _stravaAuthenticationService.GetUserToken(userStravaId);
                context["Authorization"] = $"Bearer {token?.AccessToken}";
            });
    }
}
