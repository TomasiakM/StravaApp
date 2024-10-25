using MediatR;

namespace Athletes.Application.Features.Athletes.Commands.Update;
public record UpdateAthleteCommand(
    long Id,
    string Username,
    string Firstname,
    string Lastname,
    DateTime CreatedAt,
    string ProfileMedium,
    string Profile) : IRequest<Unit>;
