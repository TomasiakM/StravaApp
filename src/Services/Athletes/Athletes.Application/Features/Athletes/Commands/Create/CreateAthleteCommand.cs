using MediatR;

namespace Athletes.Application.Features.Athletes.Commands.Create;
public record CreateAthleteCommand(
    long Id,
    string Username,
    string Firstname,
    string Lastname,
    DateTime CreatedAt,
    string ProfileMedium,
    string Profile) : IRequest<Unit>;
