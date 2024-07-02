using Common.MessageBroker.Contracts.Authorization.AuthorizeUserCode;

namespace Auth.Infrastructure.Interfaces.Services.StravaService;
internal interface IAuthorizeCodeService
{
    Task<AuthorizeCodeResponse> AuthorizeAsync(string code);
}
