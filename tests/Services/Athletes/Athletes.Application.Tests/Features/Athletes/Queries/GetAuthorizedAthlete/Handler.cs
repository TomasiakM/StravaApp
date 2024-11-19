using Athletes.Application.Features.Athletes.Queries.GetAuthorizedAthlete;
using Athletes.Application.Interfaces;
using Athletes.Application.MapperConfigurations;
using Athletes.Application.Tests.Common;
using Athletes.Domain.Aggregates.Athletes;
using Common.Domain.Exceptions;
using Common.Tests.Utils;
using Moq;
using System.Linq.Expressions;

namespace Athletes.Application.Tests.Features.Athletes.Queries.GetAuthorizedAthlete;
public class Handler
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    [Fact]
    public async Task ShouldReturnAuthorizedAthlete()
    {
        var userId = 6;
        var userIdProvider = UserIdProviderFactory.Create(userId);
        var athlete = Aggregates.CreateAthlete(userId);

        var handler = new GetAuthorizedAthleteQueryHandler(
            userIdProvider,
            _unitOfWorkMock.Object,
            MapperFactory.Create(typeof(AthleteConfiguration).Assembly));

        _unitOfWorkMock
            .Setup(e => e.Athletes.GetAsync(
                It.IsAny<Expression<Func<AthleteAggregate, bool>>>(),
                default, default, default, default))
            .ReturnsAsync(athlete);

        var response = await handler.Handle(new GetAuthorizedAthleteQuery(), default);

        _unitOfWorkMock.VerifyAll();

        Assert.Equal(athlete.StravaUserId, response.Id);
        Assert.Equal(athlete.Firstname, response.Firstname);
        Assert.Equal(athlete.Lastname, response.Lastname);
        Assert.Equal(athlete.Username, response.Username);
        Assert.Equal(athlete.CreatedAt, response.CreatedAt);
        Assert.Equal(athlete.Profile, response.Profile);
        Assert.Equal(athlete.ProfileMedium, response.ProfileMedium);
    }

    [Fact]
    public async Task ShouldThrowNotFoundExcpetion()
    {
        var userId = 2;
        var userIdProvider = UserIdProviderFactory.Create(userId);

        var handler = new GetAuthorizedAthleteQueryHandler(
            userIdProvider,
            _unitOfWorkMock.Object,
            MapperFactory.Create(typeof(AthleteConfiguration).Assembly));

        _unitOfWorkMock
            .Setup(e => e.Athletes.GetAsync(
                It.IsAny<Expression<Func<AthleteAggregate, bool>>>(),
                default, default, default, default))
            .ReturnsAsync((AthleteAggregate?)null);

        await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(new GetAuthorizedAthleteQuery(), default));

        _unitOfWorkMock.Verify(e => e.Athletes.GetAsync(
            It.IsAny<Expression<Func<AthleteAggregate, bool>>>(),
            default, default, default, default), Times.Once);
    }
}
