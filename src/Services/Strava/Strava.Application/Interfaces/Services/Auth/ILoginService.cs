using Strava.Application.Dtos.Auth;

namespace Strava.Application.Interfaces.Services.Auth;
public interface ILoginService
{
    Task<AuthResponse> LoginAsync(AuthRequest request, CancellationToken cancellationToken = default);
}
