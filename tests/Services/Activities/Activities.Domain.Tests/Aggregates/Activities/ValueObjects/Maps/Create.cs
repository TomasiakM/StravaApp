﻿using Activities.Domain.Aggregates.Activities.ValueObjects;
using Common.Domain.Models;

namespace Activities.Domain.Tests.Aggregates.Activities.ValueObjects.Maps;
public class Create
{
    [Fact]
    public void ShouldCreateMap()
    {
        var startLatLng = LatLng.Create(20.5, 11.3);
        var endLatLng = LatLng.Create(20.2, 11.6);

        var polyline = "test polyline";
        var summaryPolyline = "test summary polyline";

        var map = Map.Create(startLatLng, endLatLng, polyline, summaryPolyline);

        Assert.Equal(startLatLng, map.StartLatlng);
        Assert.Equal(endLatLng, map.EndLatlng);
        Assert.Equal(polyline, map.Polyline);
        Assert.Equal(summaryPolyline, map.SummaryPolyline);
    }

    [Fact]
    public void ShouldCreateMapWithNullValues()
    {
        var map = Map.Create(null, null, null, null);

        Assert.Null(map.StartLatlng);
        Assert.Null(map.EndLatlng);
        Assert.Null(map.Polyline);
        Assert.Null(map.SummaryPolyline);
    }
}
