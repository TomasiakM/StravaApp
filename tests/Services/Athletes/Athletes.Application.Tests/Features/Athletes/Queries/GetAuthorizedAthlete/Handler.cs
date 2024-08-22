using Athletes.Application.Features.Athletes.Queries.GetAuthorizedAthlete;
using Athletes.Application.Interfaces;
using Athletes.Application.MapperConfigurations;
using Athletes.Domain.Aggregates.Athletes;
using Common.Application.Interfaces;
using Common.Domain.Enums;
using Mapster;
using MapsterMapper;
using Moq;
using System.Linq.Expressions;

namespace Athletes.Application.Tests.Features.Athletes.Queries.GetAuthorizedAthlete;
public class Handler
{
    [Fact]
    public async Task ShouldReturnAuthorizedAthlete()
    {
        var userId = 6;

        var userIdProvider = new Mock<IUserIdProvider>();
        userIdProvider.Setup(e => e.GetUserId()).Returns(userId);

        var athlete = AthleteAggregate.Create(userId, "test", "test", "last", "profile", "profileMedium", new DateTime());
        var athleteRepository = new Mock<IAthleteRepository>();
        athleteRepository
            .Setup(e => e.GetAsync(
                It.IsAny<Expression<Func<AthleteAggregate, bool>>>(),
                It.IsAny<Expression<Func<AthleteAggregate, object>>>(),
                It.IsAny<SortOrder>(),
                It.IsAny<bool>(),
                default))
            .ReturnsAsync(athlete);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(e => e.Athletes).Returns(athleteRepository.Object);

        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(AthleteConfiguration).Assembly);
        var mapper = new Mapper(config);

        var handler = new GetAuthorizedAthleteQueryHandler(
            userIdProvider.Object,
            unitOfWork.Object,
            mapper);

        var response = await handler.Handle(new GetAuthorizedAthleteQuery(), default);

        Assert.Equal(athlete.StravaUserId, response.Id);
        Assert.Equal(athlete.Firstname, response.Firstname);
        Assert.Equal(athlete.Lastname, response.Lastname);
        Assert.Equal(athlete.Username, response.Username);
        Assert.Equal(athlete.CreatedAt, response.CreatedAt);
        Assert.Equal(athlete.Profile, response.Profile);
        Assert.Equal(athlete.ProfileMedium, response.ProfileMedium);
    }
}
