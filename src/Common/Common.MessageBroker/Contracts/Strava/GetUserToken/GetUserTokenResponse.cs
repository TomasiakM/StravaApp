namespace Common.MessageBroker.Contracts.Strava.GetUserToken;
public record GetUserTokenResponse(
    string AccessToken,
    string RefreshToken);
