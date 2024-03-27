using System.ComponentModel.DataAnnotations;

namespace Strava.Infrastructure.Settings;
public sealed class StravaSettings
{
    [Required]
    public int ClientId { get; init; }
    [Required]
    public string ClientSecret { get; init; } = default!;
}
