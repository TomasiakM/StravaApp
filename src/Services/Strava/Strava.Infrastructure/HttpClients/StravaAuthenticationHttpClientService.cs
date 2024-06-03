using Microsoft.Extensions.Options;
using Strava.Infrastructure.Settings;
using System.Text.Json;

namespace Strava.Infrastructure.HttpClients;
internal class StravaAuthenticationHttpClientService
{
    private readonly HttpClient _httpClient;
    private readonly StravaSettings _stravaSettings;

    public StravaAuthenticationHttpClientService(IHttpClientFactory httpClientFactory, IOptions<StravaSettings> stravaSettings)
    {
        _httpClient = httpClientFactory.CreateClient();
        _stravaSettings = stravaSettings.Value;

        _httpClient.BaseAddress = new Uri(_stravaSettings.BaseUrl);
    }

    public async Task<TResponse> PostRequest<TResponse>(Dictionary<string, string>? formDictionary = null, CancellationToken cancellationToken = default)
    {
        var form = new List<KeyValuePair<string, string>>
        {
            new("client_id", _stravaSettings.ClientId.ToString()),
            new("client_secret", _stravaSettings.ClientSecret),
        };

        if (formDictionary is not null)
        {
            foreach (KeyValuePair<string, string> entry in formDictionary)
            {
                form.Add(entry);
            }
        }

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/oauth/token")
        {
            Content = new FormUrlEncodedContent(form)
        };

        var res = await _httpClient.SendAsync(requestMessage, cancellationToken);
        res.EnsureSuccessStatusCode();

        var contentStream = await res.Content.ReadAsStreamAsync(cancellationToken);
        var deserializedData = JsonSerializer.Deserialize<TResponse>(contentStream, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower });

        if (deserializedData is null)
        {
            throw new Exception("Error occurred while deserializing response.");
        }

        return deserializedData;
    }
}
