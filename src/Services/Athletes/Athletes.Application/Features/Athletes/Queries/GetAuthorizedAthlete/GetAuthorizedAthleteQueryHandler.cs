using Athletes.Application.Dtos.Athletes.Responses;
using Athletes.Application.Interfaces;
using Common.Domain.Exceptions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Athletes.Application.Features.Athletes.Queries.GetAuthorizedAthlete;
internal sealed class GetAuthorizedAthleteQueryHandler
    : IRequestHandler<GetAuthorizedAthleteQuery, AthleteResponse>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAuthorizedAthleteQueryHandler(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AthleteResponse> Handle(GetAuthorizedAthleteQuery request, CancellationToken cancellationToken)
    {
        var stringId = _httpContextAccessor.HttpContext!.User.Claims
            .First(e => e.Type == ClaimTypes.NameIdentifier).Value;
        var stravaUserId = long.Parse(stringId);

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
