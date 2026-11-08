using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Enums;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Home.ResponseModels;
using PlayOn.Services.Data.Matches;
using PlayOn.Services.Data.Matches.RequestData;
using PlayOn.Services.Data.Matches.ResponseModels;
using PlayOn.Services.Mappers;


namespace PlayOn.Services
{
    public class MatchService : IMatchService
    {
        private readonly IRepository<Match> matchRepository;
        private readonly IRepository<Sport> sportRepository;

        private readonly TimeProvider timeProvider;
        private readonly ServiceMapper mapper = new();
        public MatchService(
            IRepository<Match> matchRepository,
            IRepository<Sport> sportRepository,
            TimeProvider timeProvider)
        {
            this.matchRepository = matchRepository;
            this.sportRepository = sportRepository;
            this.timeProvider = timeProvider;
        }


        public async Task<ServiceResponse<PagedResult<MatchCardResponseModel>>> GetMatchCardsAsync(
            MatchListRequestData request)
        {
            var errors = ServiceRequestDataValidator.Validate(request);

            if (errors.Any())
            {
                return ServiceResponse<PagedResult<MatchCardResponseModel>>.Failure(errors);
            }

            if (request.SportId.HasValue
                && false == await sportRepository
                    .Query()
                    .AnyAsync(x => x.Id == request.SportId.Value))
            {
                return ServiceResponse<PagedResult<MatchCardResponseModel>>
                    .Failure("Select a valid sport.", nameof(request.SportId));
            }

            var from = request.From
                ?? DateOnly.FromDateTime(timeProvider.GetLocalNow().DateTime);

            var query = matchRepository
                .Query()
                .AsSplitQuery()
                .Include(x => x.Event!)
                    .ThenInclude(x => x.Sport)
                .Include(x => x.Event!)
                    .ThenInclude(x => x.Venue)
                .Include(x => x.Participants)
                .Where(x => !x.IsDeleted && !x.IsHidden)
                .Where(x => x.Status == MatchStatus.Open || x.Status == MatchStatus.Confirmed)
                .Where(x => x.Event != null)
                .Where(x => !x.Event!.Venue.IsDeleted)
                .Where(x => x.Event!.Venue.Date >= from);

            if (request.SportId.HasValue)
            {
                var sportId = request.SportId.Value;
                query = query.Where(x => x.Event!.SportId == sportId);
            }

            var city = request.City?.Trim();

            if (!string.IsNullOrWhiteSpace(city))
            {
                query = query.Where(x =>
                    x.Event!.Venue.City.Contains(city) || city.Contains(x.Event!.Venue.City));
            }

            var term = request.SearchTerm?.Trim();

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(x =>
                    x.Title.Contains(term)
                    || x.Event!.Venue.Name.Contains(term)
                    || x.Event!.Sport.Name.Contains(term));
            }

            if (request.Skill.HasValue)
            {
                var skill = request.Skill.Value;
                query = query.Where(x => x.MinSkill <= skill && skill <= x.MaxSkill);
            }

            if (request.OnlyWithFreeSpots)
            {
                query = query.Where(x =>
                    x.Participants.Count(p => p.Status == ParticipantStatus.Joined) < x.MaxPlayers);
            }

            var totalCount = await query.CountAsync();

            var pageSize = Math.Clamp(request.PageSize, 1, 50);
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize));
            var pageNumber = Math.Clamp(request.Page, 1, totalPages);

            var ordered = request.Sort switch
            {
                MatchSort.Cheapest => query
                    .OrderBy(x => x.PricePerPlayer)
                    .ThenBy(x => x.Event!.Venue.Date)
                    .ThenBy(x => x.Event!.Venue.StartsAt)
                    .ThenBy(x => x.Id),

                MatchSort.MostSpots => query
                    .OrderByDescending(x => x.MaxPlayers
                        - x.Participants.Count(p => p.Status == ParticipantStatus.Joined))
                    .ThenBy(x => x.Event!.Venue.Date)
                    .ThenBy(x => x.Event!.Venue.StartsAt)
                    .ThenBy(x => x.Id),

                _ => query
                    .OrderBy(x => x.Event!.Venue.Date)
                    .ThenBy(x => x.Event!.Venue.StartsAt)
                    .ThenBy(x => x.Event!.Sport.Name)
                    .ThenBy(x => x.Event!.Venue.Name)
                    .ThenBy(x => x.Id)
            };

            var matches = await ordered
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = matches.Select(mapper.ToMatchCardResponse).ToList();

            return ServiceResponse<PagedResult<MatchCardResponseModel>>
                .Success(new PagedResult<MatchCardResponseModel>(items, pageNumber, pageSize, totalCount));
        }


        public async Task<ServiceResponse<MatchDetailsResponseModel>>
            GetDetailsAsync(int matchId, string? userId, bool isAdmin)
        {
            var row = await matchRepository
                .Query()
                .AsSplitQuery()
                .Where(x => x.Id == matchId && !x.IsDeleted && x.Event != null)
                .Select(x => new DetailsRow
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Status = x.Status,
                    CancelReason = x.CancelReason,
                    IsHidden = x.IsHidden,
                    DurationMinutes = x.DurationMinutes,
                    MinPlayers = x.MinPlayers,
                    MaxPlayers = x.MaxPlayers,
                    MinSkill = x.MinSkill,
                    MaxSkill = x.MaxSkill,
                    PricePerPlayer = x.PricePerPlayer,
                    OrganiserId = x.OrganiserId,
                    OrganiserName = x.Organiser.DisplayName,
                    OrganiserReliability = x.Organiser.ReliabilityScore,
                    SportName = x.Event!.Sport.Name,
                    SportIcon = x.Event!.Sport.Icon,
                    CourtName = x.Event!.Court.Name,
                    Surface = x.Event!.Court.Surface,
                    CourtCapacity = x.Event!.Court.Capacity,
                    VenueName = x.Event!.Venue.Name,
                    Address = x.Event!.Venue.Address,
                    City = x.Event!.Venue.City,
                    Date = x.Event!.Venue.Date,
                    StartsAt = x.Event!.Venue.StartsAt,
                    ClosesAt = x.Event!.Venue.ClosesAt,
                    Participants = x.Participants
                        .Where(p => p.Status != ParticipantStatus.Cancelled)
                        .OrderBy(p => p.JoinedOn)
                        .ThenBy(p => p.Id)
                        .Select(p => new ParticipantRow
                        {
                            UserId = p.UserId,
                            DisplayName = p.User.DisplayName,
                            City = p.User.City,
                            Status = p.Status,
                            Reliability = p.User.ReliabilityScore
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (row == null)
            {
                return ServiceResponse<MatchDetailsResponseModel>
                    .NotFound();
            }

            var isOrganiser = userId != null && row.OrganiserId == userId;

            if (row.IsHidden && !isOrganiser && !isAdmin)
            {
                return ServiceResponse<MatchDetailsResponseModel>
                    .NotFound();
            }

            var response = mapper.ToMatchDetailsResponse(
                row,
                (isOrganiser || isAdmin)
                    && IsOpenForChanges(row.Status, row.Date, row.StartsAt));

            return ServiceResponse<MatchDetailsResponseModel>
                .Success(response);
        }


        private bool IsOpenForChanges(MatchStatus status, DateOnly date, TimeOnly startsAt)
        {
            return (status == MatchStatus.Open || status == MatchStatus.Confirmed)
                && !HasStarted(date, startsAt);
        }

        private bool HasStarted(DateOnly date, TimeOnly startsAt)
        {
            return date.ToDateTime(startsAt) <= timeProvider.GetLocalNow().DateTime;
        }
    }
}
