﻿using Athletes.Domain.Aggregates.Athletes.ValueObjects;

namespace Athletes.Domain.Tests.Aggregates.Athletes.ValueObjects.AthleteIds;
public class Create
{
    [Fact]
    public void ShouldCreateValidAthleteId()
    {
        var athleteId = AthleteId.Create();

        Assert.NotEqual(Guid.Empty, athleteId.Value);
    }

    [Fact]
    public void ShouldCreateValidAthleteId2()
    {
        var guid = Guid.NewGuid();
        var athleteId = AthleteId.Create(guid);

        Assert.Equal(guid, athleteId.Value);
    }

    [Fact]
    public void ShouldCreateUnique2AthleteIds()
    {
        var athleteId1 = AthleteId.Create();
        var athleteId2 = AthleteId.Create();

        Assert.NotEqual(athleteId1.Value, athleteId2.Value);
        Assert.NotEqual(athleteId1, athleteId2);
    }
}