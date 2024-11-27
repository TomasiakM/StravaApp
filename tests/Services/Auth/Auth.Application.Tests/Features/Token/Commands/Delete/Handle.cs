using Auth.Application.Features.Token.Commands.Delete;
using Auth.Application.Interfaces;
using Auth.Domain.Aggregates.Token;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Auth.Application.Tests.Features.Token.Commands.Delete;
public class Handle
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<DeleteTokenCommandHandler>> _loggerMock = new();

    [Fact]
    public async Task ShouldDeleteUserTokenWhenToken()
    {
        var command = new DeleteTokenCommand(1);

        var handler = new DeleteTokenCommandHandler(
            _unitOfWorkMock.Object,
            _loggerMock.Object);

        _unitOfWorkMock
            .Setup(e => e.Tokens.GetAsync(
                It.IsAny<Expression<Func<TokenAggregate, bool>>>(),
                default, default, default, default))
            .ReturnsAsync(TokenAggregate.Create(1, "test", "test2", 10));

        await handler.Handle(command, default);

        _unitOfWorkMock.Verify(e => e.Tokens.Delete(It.IsAny<TokenAggregate>()), Times.Once);
        _unitOfWorkMock.Verify(e => e.SaveChangesAsync(default), Times.Once);
    }
}
