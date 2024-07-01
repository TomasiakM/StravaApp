using Auth.Application.Dtos;
using Auth.Application.Interfaces;
using Auth.Application.Interfaces.Services;
using Auth.Domain.Aggregates.Token;
using Auth.Infrastructure.Interfaces;
using Auth.Infrastructure.Interfaces.Services.StravaService;
using Common.MessageBroker.Contracts.Athletes;
using MapsterMapper;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Auth.Infrastructure.Services.Auth;
internal sealed class LoginService : ILoginService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBus _bus;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthorizeCodeService _authorizeCodeService;

    public LoginService(IUnitOfWork unitOfWork, IBus bus, IMapper mapper, ITokenService tokenService, IHttpContextAccessor httpContextAccessor, IAuthorizeCodeService authorizeCodeService)
    {
        _unitOfWork = unitOfWork;
        _bus = bus;
        _mapper = mapper;
        _tokenService = tokenService;
        _httpContextAccessor = httpContextAccessor;
        _authorizeCodeService = authorizeCodeService;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var authorizedData = await _authorizeCodeService.AuthorizeAsync(request.Code);

        var token = await _unitOfWork.Tokens.GetAsync(
            filter: e => e.StravaUserId == authorizedData.Athlete.Id,
            cancellationToken: cancellationToken);

        if (token is not null)
        {
            token.Update(
                authorizedData.RefreshToken,
                authorizedData.AccessToken,
                authorizedData.ExpiresAt);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        if (token is null)
        {
            token = TokenAggregate.Create(
                authorizedData.Athlete.Id,
                authorizedData.RefreshToken,
                authorizedData.AccessToken,
                authorizedData.ExpiresAt);

            _unitOfWork.Tokens.Add(token);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _bus.Publish(new NewAthleteLoggedInEvent(authorizedData.Athlete.Id));
        }

        await _bus.Publish(_mapper.Map<ReceivedAthleteDataEvent>(authorizedData.Athlete));

        var accessToken = _tokenService.GenerateToken(token.StravaUserId);
        await AddRefreshCookieToContext(token);

        return new LoginResponse(
            accessToken,
            _mapper.Map<AthleteResponse>(authorizedData.Athlete));
    }

    private async Task AddRefreshCookieToContext(TokenAggregate token)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, token.StravaUserId.ToString()),
        };

        var claimsIdentity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        await _httpContextAccessor.HttpContext!
            .SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
    }
}
