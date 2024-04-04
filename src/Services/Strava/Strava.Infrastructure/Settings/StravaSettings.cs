using System.ComponentModel.DataAnnotations;

namespace Strava.Infrastructure.Settings;
public sealed class StravaSettings
{
    [Required]
    public string BaseUrl { get; set; } = default!;
    [Required]
    public int ClientId { get; set; }
    [Required]
    public string ClientSecret { get; set; } = default!;
}
