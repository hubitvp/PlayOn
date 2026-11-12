using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Enums;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Matches;
using PlayOn.Services.Data.Matches.RequestData;
using PlayOn.Services.Data.Matches.ResponseModels;
using PlayOn.Services.DataBuilders;


namespace PlayOn.Services
{
    public class MatchService : IMatchService
    {
        private readonly IRepository<Match> matchRepository;
        private readonly IRepository<Sport> sportRepository;
        private readonly IRepository<Court> courtRepository;
        private readonly IRepository<Venue> venueRepository;

        private readonly TimeProvider timeProvider;
        public MatchService(
            IRepository<Match> matchRepository,
            IRepository<Sport> sportRepository,
            IRepository<Court> courtRepository,
            IRepository<Venue> venueRepository,
            TimeProvider timeProvider)
        {
            this.matchRepository = matchRepository;
            this.sportRepository = sportRepository;
            this.timeProvider = timeProvider;
            this.courtRepository = courtRepository;
            this.venueRepository = venueRepository;
        }


        public async Task<ServiceResponse<PageResult<MatchCardResponseModel>>>
            GetMatchCardsAsync(MatchSearchRequestData request)
        {
            var errors = ServiceRequestDataValidator.Validate(request);

            if (errors.Any())
            {
                return ServiceResponse<PageResult<MatchCardResponseModel>>.Failure(errors);
            }

            if (request.SportId.HasValue
                && false == await sportRepository
                    .Query()
                    .AnyAsync(x => x.Id == request.SportId.Value))
            {
                return ServiceResponse<PageResult<MatchCardResponseModel>>
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
                .Select(MatchDataBuilder.MatchCardProjection())
                .ToListAsync();

            return ServiceResponse<PageResult<MatchCardResponseModel>>
                .Success(new PageResult<MatchCardResponseModel>(projectedItems, pageNumber, pageSize, totalCount));
        }


        public async Task<ServiceResponse<MatchDetailsResponseModel>>
            GetDetailsAsync(int matchId, string? userId, bool isAdmin)
        {
            var row = await matchRepository
                .Query()
                .AsSplitQuery()
                .Where(x => x.Id == matchId && !x.IsDeleted && x.Event != null)
                .Select(MatchDataBuilder.MatchDetailsRowProjection())
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

            var response = MatchDataBuilder.MatchDetailsRowToResponseModel(
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
                .Select(MatchDataBuilder.MatchEditRowProjection())
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

            var responseModel = MatchDataBuilder.MatchEditRowToResponseModel(row);

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
                .Select(MatchDataBuilder.MatchEditRowProjection())
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

            MatchDataBuilder.MatchUpdateApplyToEntity(match, request);

            await matchRepository.SaveChangesAsync();

            return ServiceResponse<int>.Success(request.MatchId);
        }

        public async Task<ServiceResponse<int>>
            CreateAsync(
                MatchCreateRequestData request)
        {
            var errors = ServiceRequestDataValidator.Validate(request);

            if (errors.Any())
            {
                return ServiceResponse<int>.Failure(errors);
            }

            if (!await sportRepository.Query().AnyAsync(x => x.Id == request.SportId))
            {
                return ServiceResponse<int>.Failure("Select a valid sport.", nameof(request.SportId));
            }

            var court = await courtRepository
                .Query()
                .Where(x => x.Id == request.CourtId)
                .Select(MatchDataBuilder.MatchCourtOptionProjection())
                .FirstOrDefaultAsync();

            if (court == null)
            {
                return ServiceResponse<int>.Failure("Select a valid court.", nameof(request.CourtId));
            }

            if (!court.SportIds.Contains(request.SportId))
            {
                return ServiceResponse<int>.Failure(
                    "This court cannot be used for the selected sport.", nameof(request.CourtId));
            }

            if (request.MaxPlayers > court.Capacity)
            {
                return ServiceResponse<int>.Failure(
                    $"This court fits at most {court.Capacity} players.", nameof(request.MaxPlayers));
            }

            var slot = await venueRepository
                .Query()
                .Where(x => x.Id == request.VenueId)
                .Select(MatchDataBuilder.SlotRowProjection())
                .FirstOrDefaultAsync();

            if (slot == null || slot.IsDeleted)
            {
                return ServiceResponse<int>
                    .Failure(
                        "Select a valid date and time slot.",
                        nameof(request.VenueId));
            }

            if (slot.IsTaken)
            {
                return ServiceResponse<int>
                    .Failure(
                        "This slot is already taken.",
                        nameof(request.VenueId));
            }

            if (HasStarted(slot.Date, slot.StartsAt))
            {
                return ServiceResponse<int>.Failure("This slot is in the past.", nameof(request.VenueId));
            }

            var match = MatchDataBuilder.MatchCreateToEntity(
                request,
                timeProvider.GetLocalNow().LocalDateTime);

            await matchRepository.AddAsync(match);
            await matchRepository.SaveChangesAsync();

            return ServiceResponse<int>
                .Success(match.Id);
        }


        public async Task<MatchFormOptionsResponseModel> GetFormOptionsAsync()
        {
            DateOnly today = DateOnly.FromDateTime(timeProvider.GetLocalNow().LocalDateTime);

            var sports = await sportRepository
                .Query()
                .OrderBy(x => x.Name)
                .Select(SportDataBuilder.SportProjection())
                .ToListAsync();

            var courts = await courtRepository
                .Query()
                .OrderBy(x => x.Name)
                .Select(MatchDataBuilder.MatchCourtOptionProjection())
                .ToListAsync();

            var slots = await venueRepository
                .Query()
                .Where(x => !x.IsDeleted && x.Event == null && x.Date >= today)
                .OrderBy(x => x.Date)
                .ThenBy(x => x.StartsAt)
                .ThenBy(x => x.Id)
                .Take(100)
                .Select(MatchDataBuilder.VenueSlotProjection())
                .ToListAsync();

            return MatchDataBuilder.MatchFormOptionsToResponseModel(sports, courts, slots);
        }
        public async Task<ServiceResponse<MatchEditResponseModel>>
            GetForCancelAsync(
                int matchId,
                string actorId,
                bool actorIsAdmin)
        {
            return await GetForEditAsync(matchId, actorId, actorIsAdmin);
        }


        public async Task<ServiceResponse<int>>
            CancelAsync(MatchCancelRequestData request)
        {
            var errors = ServiceRequestDataValidator.Validate(request);

            if (errors.Any())
            {
                return ServiceResponse<int>.Failure(errors);
            }

            var row = await matchRepository
                .Query()
                .Where(x => x.Id == request.MatchId && x.Event != null)
                .Select(MatchDataBuilder.MatchEditRowProjection())
                .FirstOrDefaultAsync();

            if (row == null || row.IsDeleted)
            {
                return ServiceResponse<int>.NotFound();
            }

            if (row.OrganiserId != request.ActorId && !request.ActorIsAdmin)
            {
                return ServiceResponse<int>
                    .Forbidden();
            }

            if (!IsOpenForChanges(row.Status, row.Date, row.StartsAt))
            {
                return ServiceResponse<int>
                    .Conflict("This match is cancelled, finished or already started, so it can no longer be changed.");
            }

            var match = await matchRepository
                .Query(asNoTracking: false)
                .FirstAsync(x => x.Id == row!.Id);

            MatchDataBuilder.MatchCancelApplyToEntity(match, request);

            await matchRepository.SaveChangesAsync();

            return ServiceResponse<int>.Success(match.Id);
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
