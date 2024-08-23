using Common.Application.Interfaces;
using Moq;

namespace Common.Tests.Utils;
public static class UserIdProviderFactory
{
    public static IUserIdProvider Create(long userId)
    {
        var userIdProvider = new Mock<IUserIdProvider>();

        userIdProvider.Setup(e => e.GetUserId()).Returns(userId);

        return userIdProvider.Object;
    }
}
