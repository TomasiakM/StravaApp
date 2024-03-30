using Athletes.Application.Dtos.Athletes.Responses;
using MediatR;

namespace Athletes.Application.Features.Athletes.Queries.GetAuthorizedAthlete;
public record GetAuthorizedAthleteQuery() : IRequest<AthleteResponse>;
