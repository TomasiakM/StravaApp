using Athletes.Application.Dtos.Athletes.Responses;
using Athletes.Application.Interfaces;
using Common.Application.Interfaces;
using Common.Domain.Exceptions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Athletes.Application.Features.Athletes.Queries.GetAuthorizedAthlete;
internal sealed class GetAuthorizedAthleteQueryHandler
    : IRequestHandler<GetAuthorizedAthleteQuery, AthleteResponse>
{
    private readonly IUserIdProvider _userIdProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAuthorizedAthleteQueryHandler(IUserIdProvider userIdProvider, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _userIdProvider = userIdProvider;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AthleteResponse> Handle(GetAuthorizedAthleteQuery request, CancellationToken cancellationToken)
    {
        var stravaUserId = _userIdProvider.GetUserId();

        var athlete = await _unitOfWork.Athletes
            .FindAsync(e => e.StravaUserId == stravaUserId, cancellationToken);

        if (athlete is null)
        {
            throw new NotFoundException(stravaUserId);
        }

        var dto = _mapper.Map<AthleteResponse>(athlete);

        return dto;
    }
}
