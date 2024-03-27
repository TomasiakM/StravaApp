namespace Strava.Infrastructure.Interfaces;
internal interface ITokenService
{
    Task<string> GenerateToken(long StravaUserId);
}
