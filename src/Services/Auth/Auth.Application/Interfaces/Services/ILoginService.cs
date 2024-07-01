using Auth.Application.Dtos;

namespace Auth.Application.Interfaces.Services;
public interface ILoginService
{
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}
