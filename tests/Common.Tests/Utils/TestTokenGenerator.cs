using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Common.Tests.Utils;
public class TestTokenGenerator
{
    public static string Create(int userId = 1)
    {
        var data = Encoding.UTF8.GetBytes("SuperSecretTokenSecretForTesting");
        var securityKey = new SymmetricSecurityKey(data);

        var claims = new Dictionary<string, object>
        {
            [ClaimTypes.NameIdentifier] = userId,
        };

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = "Issuer",
            Audience = "Audience",
            Claims = claims,
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(10),
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var handler = new JsonWebTokenHandler();
        handler.SetDefaultTimesOnTokenCreation = false;

        return handler.CreateToken(descriptor);
    }
}
