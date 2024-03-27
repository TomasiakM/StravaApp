namespace Strava.Infrastructure.Interfaces;
internal interface ITokenService
{
    string GenerateToken(long stravaUserId);
}
