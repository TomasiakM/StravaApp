using Activities.Application.Features.Activities.Queries.GetAllActivities;
using Activities.Application.Interfaces;
using Activities.Application.MapperConfigurations;
using Activities.Domain.Aggregates.Activities;
using Activities.Domain.Aggregates.Activities.ValueObjects;
using Common.Application.Interfaces;
using Common.Domain.Enums;
using Common.Domain.Models;
using Common.Tests.Utils;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Activities.Application.Tests.Features.Activities.Queries.GetAllActivities;
public class Handler
{
    [Fact]
    public async Task ShouldReturnActivitiesResponseList()
    {
        var stravaUserId = 5;

        var mockRepository = new Mock<IActivityRepository>();
        mockRepository
            .Setup(e => e.GetAllAsync(
                It.IsAny<Expression<Func<ActivityAggregate, bool>>>(),
                It.IsAny<Expression<Func<ActivityAggregate, object>>>(),
                It.IsAny<SortOrder>(),
                It.IsAny<bool>(),
                default))
            .ReturnsAsync(new List<ActivityAggregate> {
                ActivityAggregate.Create(
                    1, stravaUserId, "test", "test", SportType.Canoeing,
                    false, 20, 20, 20, 300, 300,
                    Speed.Create(20, 13),
                    Time.Create(30, 30, new DateTime(), new DateTime()),
                    Watts.Create(false, 220, 193),
                    Heartrate.Create(true, 180, 130),
                    Map.Create(LatLng.Create(0, 0), LatLng.Create(0,0), "test", "test"))
            });

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(e => e.Activities).Returns(mockRepository.Object);

        var mockUserProvider = new Mock<IUserIdProvider>();
        mockUserProvider.Setup(e => e.GetUserId()).Returns(stravaUserId);

        var queryHandler = new GetAllActivitiesQueryHandler(
            Mock.Of<ILogger<GetAllActivitiesQueryHandler>>(),
            mockUnitOfWork.Object,
            mockUserProvider.Object,
            MapperFactory.Create(typeof(ActivitiesConfiguration).Assembly));

        var activitiesDtos = await queryHandler.Handle(new GetAllActivitiesQuery(), default);

        Assert.Single(activitiesDtos);
        Assert.NotNull(activitiesDtos.First());
    }
}
