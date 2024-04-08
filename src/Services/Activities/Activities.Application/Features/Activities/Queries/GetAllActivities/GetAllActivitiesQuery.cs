using Activities.Application.Dtos.Activities;
using MediatR;

namespace Activities.Application.Features.Activities.Queries.GetAllActivities;
public record GetAllActivitiesQuery() : IRequest<IEnumerable<ActivityResponse>>;
