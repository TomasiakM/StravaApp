using Athletes.Domain.Aggregates.Athletes.ValueObjects;
using Common.Domain.DDD;

namespace Athletes.Domain.Aggregates.Athletes;
public sealed class AthleteAggregate : AggregateRoot<AthleteId>
{
    public long StravaUserId { get; init; }
    public string Username { get; private set; }
    public string Firstname { get; private set; }
    public string Lastname { get; private set; }
    public string Profile { get; private set; }
    public string ProfileMedium { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private AthleteAggregate(long stravaUserId, string username, string firstname, string lastname, string profile, string profileMedium, DateTime createdAt)
        : base(AthleteId.Create())
    {
        StravaUserId = stravaUserId;
        Username = username;
        Firstname = firstname;
        Lastname = lastname;
        Profile = profile;
        ProfileMedium = profileMedium;
        CreatedAt = createdAt;
    }

    public static AthleteAggregate Create(long stravaUserId, string username, string firstname, string lastname, string profile, string profileMedium, DateTime createdAt)
        => new(stravaUserId, username, firstname, lastname, profile, profileMedium, createdAt);
}
