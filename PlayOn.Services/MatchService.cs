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
        public MatchService(
            IRepository<Match> matchRepository,
            IRepository<Sport> sportRepository,
            TimeProvider timeProvider)
        {
            this.matchRepository = matchRepository;
            this.sportRepository = sportRepository;
            this.timeProvider = timeProvider;
        }


        public async Task<ServiceResponse<PagedResult<MatchCardResponseModel>>>
            GetMatchCardsAsync(MatchListRequestData request)
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

            var projectedItems = await ordered
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(HomeMapper.MatchCardProjection())
                .ToListAsync();

            return ServiceResponse<PagedResult<MatchCardResponseModel>>
                .Success(new PagedResult<MatchCardResponseModel>(projectedItems, pageNumber, pageSize, totalCount));
        }


        public async Task<ServiceResponse<MatchDetailsResponseModel>>
            GetDetailsAsync(int matchId, string? userId, bool isAdmin)
        {
            var row = await matchRepository
                .Query()
                .AsSplitQuery()
                .Where(x => x.Id == matchId && !x.IsDeleted && x.Event != null)
                .Select(MatchMapper.DetailsProjection())
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

            var response = MatchMapper.ToMatchDetailsResponse(
                row,
                (isOrganiser || isAdmin)
                    && IsOpenForChanges(row.Status, row.Date, row.StartsAt));

            return ServiceResponse<MatchDetailsResponseModel>
                .Success(response);
        }

        public async Task<ServiceResponse<MatchEditResponseModel>>
            GetForEditAsync(
                int matchId,
                string actorId,
                bool actorIsAdmin)
        {
            var row = await matchRepository
                .Query()
                .Where(x => x.Id == matchId && x.Event != null)
                .Select(x => new EditRow
                {
                    Id = x.Id,
                    OrganiserId = x.OrganiserId,
                    IsDeleted = x.IsDeleted,
                    Status = x.Status,
                    Title = x.Title,
                    Description = x.Description,
                    DurationMinutes = x.DurationMinutes,
                    MinPlayers = x.MinPlayers,
                    MaxPlayers = x.MaxPlayers,
                    MinSkill = x.MinSkill,
                    MaxSkill = x.MaxSkill,
                    PricePerPlayer = x.PricePerPlayer,
                    RowVersion = x.RowVersion,
                    SportName = x.Event!.Sport.Name,
                    SportIcon = x.Event!.Sport.Icon,
                    CourtId = x.Event!.CourtId,
                    CourtName = x.Event!.Court.Name,
                    CourtCapacity = x.Event!.Court.Capacity,
                    VenueName = x.Event!.Venue.Name,
                    City = x.Event!.Venue.City,
                    Date = x.Event!.Venue.Date,
                    StartsAt = x.Event!.Venue.StartsAt,
                    ClosesAt = x.Event!.Venue.ClosesAt,
                    JoinedCount = x.Participants.Count(p => p.Status == ParticipantStatus.Joined)
                })
                .SingleOrDefaultAsync();

            if (row == null || row.IsDeleted)
            {
                return ServiceResponse<MatchEditResponseModel>
                    .NotFound();
            }

            if (row.OrganiserId != actorId && !actorIsAdmin)
            {
                return ServiceResponse<MatchEditResponseModel>
                    .Forbidden();
            }

            if (!IsOpenForChanges(row.Status, row.Date, row.StartsAt))
            {
                return ServiceResponse<MatchEditResponseModel>
                    .Conflict("This match is cancelled, finished or already started, so it can no longer be changed.");
            }

            var responseModel = new MatchEditResponseModel
            {
                Id = row.Id,
                Title = row.Title,
                Description = row.Description,
                DurationMinutes = row.DurationMinutes,
                MinPlayers = row.MinPlayers,
                MaxPlayers = row.MaxPlayers,
                MinSkill = row.MinSkill,
                MaxSkill = row.MaxSkill,
                PricePerPlayer = row.PricePerPlayer,
                RowVersion = row.RowVersion,
                SportName = row.SportName,
                SportIcon = row.SportIcon,
                CourtName = row.CourtName,
                CourtCapacity = row.CourtCapacity,
                VenueName = row.VenueName,
                City = row.City,
                Date = row.Date,
                StartsAt = row.StartsAt,
                ClosesAt = row.ClosesAt,
                JoinedCount = row.JoinedCount
            };

            return ServiceResponse<MatchEditResponseModel>
                .Success(responseModel);
        }

        public async Task<ServiceResponse<int>>
            UpdateAsync(MatchUpdateRequestData request)
        {
            var errors = ServiceRequestDataValidator.Validate(request);

            if (errors.Any())
            {
                return ServiceResponse<int>.Failure(errors);
            }

            var row = await matchRepository
                .Query()
                .Where(x => x.Id == request.MatchId && x.Event != null)
                .Select(x => new EditRow
                {
                    Id = x.Id,
                    OrganiserId = x.OrganiserId,
                    IsDeleted = x.IsDeleted,
                    Status = x.Status,
                    Title = x.Title,
                    Description = x.Description,
                    DurationMinutes = x.DurationMinutes,
                    MinPlayers = x.MinPlayers,
                    MaxPlayers = x.MaxPlayers,
                    MinSkill = x.MinSkill,
                    MaxSkill = x.MaxSkill,
                    PricePerPlayer = x.PricePerPlayer,
                    RowVersion = x.RowVersion,
                    SportName = x.Event!.Sport.Name,
                    SportIcon = x.Event!.Sport.Icon,
                    CourtId = x.Event!.CourtId,
                    CourtName = x.Event!.Court.Name,
                    CourtCapacity = x.Event!.Court.Capacity,
                    VenueName = x.Event!.Venue.Name,
                    City = x.Event!.Venue.City,
                    Date = x.Event!.Venue.Date,
                    StartsAt = x.Event!.Venue.StartsAt,
                    ClosesAt = x.Event!.Venue.ClosesAt,
                    JoinedCount = x.Participants.Count(p => p.Status == ParticipantStatus.Joined)
                })
                .FirstOrDefaultAsync();

            if (row == null || row.IsDeleted)
            {
                return ServiceResponse<int>.NotFound();
            }

            if (row.OrganiserId != request.ActorId && !request.ActorIsAdmin)
            {
                return ServiceResponse<int>.Forbidden();
            }

            if (!IsOpenForChanges(row.Status, row.Date, row.StartsAt))
            {
                return ServiceResponse<int>.Conflict(
                    "This match is cancelled, finished or already started, so it can no longer be changed.");
            }


            if (request.MaxPlayers < row!.JoinedCount)
            {
                return ServiceResponse<int>.Failure(
                    $"{row.JoinedCount} players have already joined, so the maximum cannot be lower.",
                    nameof(request.MaxPlayers));
            }

            if (request.MaxPlayers > row.CourtCapacity)
            {
                return ServiceResponse<int>.Failure(
                    $"This court fits at most {row.CourtCapacity} players.", nameof(request.MaxPlayers));
            }

            var match = await matchRepository
                .Query(asNoTracking: false)
                .FirstAsync(x => x.Id == row.Id);

            match.Title = request.Title.Trim();
            match.Description = request.Description;
            match.DurationMinutes = request.DurationMinutes;
            match.MinPlayers = request.MinPlayers;
            match.MaxPlayers = request.MaxPlayers;
            match.MinSkill = request.MinSkill;
            match.MaxSkill = request.MaxSkill;
            match.PricePerPlayer = request.PricePerPlayer;

            await matchRepository.SaveChangesAsync();

            return ServiceResponse<int>.Success(request.MatchId);
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
