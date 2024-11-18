using Athletes.Domain.Aggregates.Athletes;

namespace Athletes.Application.Tests.Common;
internal static class Aggregates
{
    public static AthleteAggregate CreateAthlete(long stravaUserId)
    {
        return AthleteAggregate.Create(stravaUserId, "Username", "Firstname", "Lastname", "Profile", "ProfileMedium", new DateTime(2022, 1, 1));
    }
}
