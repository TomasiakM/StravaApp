﻿using Strava.Contracts.Athlete;

namespace Strava.Contracts.Activity;
public record StravaActivitySummaryResponse(
    long Id,
    DateTime StartDate,
    StravaAthleteMetaResponse Athlete);
