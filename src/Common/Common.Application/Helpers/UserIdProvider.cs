using Common.Application.Interfaces;
using Common.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Common.Application.Helpers;
internal sealed class UserIdProvider : IUserIdProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserIdProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public long GetUserId()
    {
        var stringId = _httpContextAccessor.HttpContext!.User.Claims
            .FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(stringId))
        {
            throw new UnauthorizedException();
        }

        var stravaUserId = long.Parse(stringId);

        return stravaUserId;
    }
}
