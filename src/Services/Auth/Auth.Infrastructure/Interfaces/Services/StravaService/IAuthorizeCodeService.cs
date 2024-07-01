using Common.MessageBroker.Contracts.Authorization.AuthorizeUserCode;

namespace Auth.Infrastructure.Interfaces.Services.StravaService;
internal interface IAuthorizeCodeService
{
    Task<AuthorizeUserCodeResponse> AuthorizeAsync(string code);
}
