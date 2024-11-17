using MediatR;

namespace Activities.Application.Features.Activities.Queries.GetAllActivities;
public record GetAllActivitiesQuery() : IRequest<IEnumerable<GetAllActivitiesQueryResponse>>;
