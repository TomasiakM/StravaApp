using Activities.Domain.Aggregates.Activities.ValueObjects;
using Common.Domain.DDD;
using Common.Domain.Enums;

namespace Activities.Domain.Aggregates.Activities;
public sealed class ActivityAggregate : AggregateRoot<ActivityId>
{
    public long StravaId { get; init; }
    public long StravaUserId { get; init; }

    public string Name { get; private set; }
    public string DeviceName { get; private set; }
    public SportType SportType { get; private set; }
    public bool Private { get; private set; }

    public float Distance { get; private set; }
    public float TotalElevationGain { get; private set; }
    public float AverageCadence { get; private set; }

    public float Kilojoules { get; private set; }
    public float Calories { get; private set; }

    public Speed Speed { get; private set; }
    public Time Time { get; private set; }
    public Watts Watts { get; private set; }
    public Heartrate Heartrate { get; private set; }
    public Map Map { get; private set; }

    private ActivityAggregate(long stravaId, long stravaUserId, string name, string deviceName, SportType sportType, bool @private, float distance, float totalElevationGain, float averageCadence, float kilojoules, float calories, Speed speed, Time time, Watts watts, Heartrate heartrate, Map map)
        : base(ActivityId.Create())
    {
        StravaId = stravaId;
        StravaUserId = stravaUserId;

        Name = name;
        DeviceName = deviceName;
        SportType = sportType;
        Private = @private;

        Distance = distance;
        TotalElevationGain = totalElevationGain;
        AverageCadence = averageCadence;

        Kilojoules = kilojoules;
        Calories = calories;

        Speed = speed;
        Time = time;
        Watts = watts;
        Heartrate = heartrate;
        Map = map;
    }

    public static ActivityAggregate Create(long stravaId, long stravaUserId, string name, string deviceName, SportType sportType, bool @private, float distance, float totalElevationGain, float averageCadence, float kilojoules, float calories, Speed speed, Time time, Watts watts, Heartrate heartrate, Map map)
        => new(stravaId, stravaUserId, name, deviceName, sportType, @private, distance, totalElevationGain, averageCadence, kilojoules, calories, speed, time, watts, heartrate, map);

    public void Update(string name, string deviceName, SportType sportType, bool @private, float distance, float totalElevationGain, float averageCadence, float kilojoules, float calories, Speed speed, Time time, Watts watts, Heartrate heartrate, Map map)
    {
        Name = name;
        DeviceName = deviceName;
        SportType = sportType;
        Private = @private;

        Distance = distance;
        TotalElevationGain = totalElevationGain;
        AverageCadence = averageCadence;

        Kilojoules = kilojoules;
        Calories = calories;

        Speed = speed;
        Time = time;
        Watts = watts;
        Heartrate = heartrate;
        Map = map;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private ActivityAggregate() : base(ActivityId.Create()) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
