using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PlayOn.Common.Enums;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Match.RequestData;
using PlayOn.Services.Data.Match.ResponseModels;
using PlayOn.Services.Data.Sport.ResponseModels;
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
            this.courtRepository = courtRepository;
            this.venueRepository = venueRepository;
            this.timeProvider = timeProvider;
        }


        public async Task<ServiceResponse<MatchResponseModel>>
            GetMainResponseModelAsync(MatchSearchRequestData request, PageRequestData pageRequestData)
        {
            var errors = ServiceRequestDataValidator.Validate(request);

            if (errors.Any())
            {
                return ServiceResponse<MatchResponseModel>.Failure(errors);
            }


            var pageResponse = await GetSinglePageAsync(request, pageRequestData);

            if (!pageResponse.IsSuccess)
            {
                return ServiceResponse<MatchResponseModel>.Failure(pageResponse.Errors);
            }

            var optionsResponse = await GetOptionsAsync(request.From);
            if (!optionsResponse.IsSuccess)
            {
                return ServiceResponse<MatchResponseModel>.Failure(optionsResponse.Errors);
            }

            var response = MatchDataBuilder.ToMatchResponseModel(
                pageResponse.ResponseData,
                optionsResponse.ResponseData);

            return ServiceResponse<MatchResponseModel>.Success(response);
        }




        public async Task<ServiceResponse<PageResult<MatchCardResponseModel>>>
            GetSinglePageAsync(
                MatchSearchRequestData searchRequestData,
                PageRequestData pageRequestData)
        {

            var errors = ServiceRequestDataValidator.Validate(searchRequestData);

            if (errors.Any())
            {
                return ServiceResponse<PageResult<MatchCardResponseModel>>.Failure(errors);
            }

            if (searchRequestData.SportId.HasValue
                && false == await sportRepository
                    .Query()
                    .AnyAsync(x => x.Id == searchRequestData.SportId.Value))
            {
                return ServiceResponse<PageResult<MatchCardResponseModel>>
                    .Failure("Select a valid sport.", nameof(searchRequestData.SportId));
            }


            var from = searchRequestData.From
                ?? DateOnly.FromDateTime(timeProvider.GetLocalNow().LocalDateTime);

            var query = matchRepository
                .Query()
                .AsSplitQuery()
                .Where(x => !x.IsDeleted && !x.IsHidden)
                .Where(x => x.Status == MatchStatus.Open || x.Status == MatchStatus.Confirmed)
                .Where(x => x.Event != null)
                .Where(x => !x.Event!.Venue.IsDeleted)
                .Where(x => x.Event!.Venue.Date >= from);

            if (searchRequestData.SportId.HasValue)
            {
                var sportId = searchRequestData.SportId.Value;
                query = query.Where(x => x.Event!.SportId == sportId);
            }

            var city = searchRequestData.City?.Trim();

            if (!string.IsNullOrWhiteSpace(city))
            {
                query = query.Where(x =>
                    x.Event!.Venue.City.Contains(city) || city.Contains(x.Event!.Venue.City));
            }

            var term = searchRequestData.SearchTerm?.Trim();

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(x =>
                    x.Title.Contains(term)
                    || x.Event!.Venue.Name.Contains(term)
                    || x.Event!.Sport.Name.Contains(term));
            }

            if (searchRequestData.Skill.HasValue)
            {
                var skill = searchRequestData.Skill.Value;
                query = query.Where(x => x.MinSkill <= skill && skill <= x.MaxSkill);
            }

            if (searchRequestData.OnlyWithFreeSpots)
            {
                query = query.Where(x =>
                    x.Participants.Count(p => p.Status == ParticipantStatus.Joined) < x.MaxPlayers);
            }

            var totalCount = await query.CountAsync();

            var pageSize = Math.Clamp(pageRequestData.PageSize, 1, 50);
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize));
            var pageNumber = Math.Clamp(pageRequestData.Page, 1, totalPages);

            var ordered = searchRequestData.Sort switch
            {
                MatchSort.Cheapest => query
                    .OrderBy(x => x.PricePerPlayer)
                    .ThenBy(x => x.Event!.Venue.Date)
                    .ThenBy(x => x.Event!.Venue.StartsAt),

                MatchSort.MostSpots => query
                    .OrderByDescending(
                        x =>
                            x.MaxPlayers - x.Participants.Count(
                                p => p.Status == ParticipantStatus.Joined))
                    .ThenBy(x => x.Event!.Venue.Date)
                    .ThenBy(x => x.Event!.Venue.StartsAt),

                _ => query
                    .OrderBy(x => x.Event!.Venue.Date)
                    .ThenBy(x => x.Event!.Venue.StartsAt)
                    .ThenBy(x => x.Event!.Sport.Name)
                    .ThenBy(x => x.Event!.Venue.Name)
            };


            var projectedItems = await ordered
                .ThenBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(MatchDataBuilder.MatchCardProjection())
                .ToListAsync();

            var page = new PageResult<MatchCardResponseModel>(projectedItems, pageNumber, pageSize, totalCount);

            return ServiceResponse<PageResult<MatchCardResponseModel>>.Success(page);
        }


        public async Task<ServiceResponse<IReadOnlyList<SportOptionResponseModel>>>
            GetSportOptionsAsync()
        {

            var sportOptions = await sportRepository
                .Query()
                .OrderBy(x => x.Name)
                .Select(MatchDataBuilder.SportToSportOptionProjection())
                .ToListAsync();

            return ServiceResponse<IReadOnlyList<SportOptionResponseModel>>.Success(sportOptions);
        }



        public async Task<ServiceResponse<IReadOnlyList<VenueOptionResponseModel>>>
            GetVenueOptionsAsync(DateOnly? from)
        {
            var today = DateOnly.FromDateTime(timeProvider.GetLocalNow().LocalDateTime);

            DateOnly fromDate = today.AddDays(1);

            if (from.HasValue && from.Value > today)
            {
                fromDate = from.Value;
            }

            var venueOptions = await venueRepository
                .Query()
                .Where(x => !x.IsDeleted && x.Event == null && x.Date >= fromDate)
                .OrderBy(x => x.Date)
                .ThenBy(x => x.StartsAt)
                .ThenBy(x => x.Id)
                .Select(MatchDataBuilder.VenueToVenueOptionProjection())
                .ToListAsync();

            return ServiceResponse<IReadOnlyList<VenueOptionResponseModel>>.Success(venueOptions);
        }


        public async Task<ServiceResponse<IReadOnlyList<CourtOptionResponseModel>>>
            GetCourtOptionsAsync()
        {
            var courtOptions = await courtRepository
                .Query()
                .OrderBy(x => x.Name)
                .Select(MatchDataBuilder.CourtToCourtOptionProjection())
                .ToListAsync();
            return ServiceResponse<IReadOnlyList<CourtOptionResponseModel>>.Success(courtOptions);
        }



        public async Task<ServiceResponse<MatchOptionsResponseModel>>
            GetOptionsAsync(DateOnly? from)
        {
            var sportOptions = await GetSportOptionsAsync();
            if (!sportOptions.IsSuccess)
            {
                return ServiceResponse<MatchOptionsResponseModel>.Failure(sportOptions.Errors);
            }

            var courts = await GetCourtOptionsAsync();
            if (!courts.IsSuccess)
            {
                return ServiceResponse<MatchOptionsResponseModel>.Failure(courts.Errors);
            }

            var venues = await GetVenueOptionsAsync(from);
            if (!venues.IsSuccess)
            {
                return ServiceResponse<MatchOptionsResponseModel>.Failure(venues.Errors);
            }

            var response = MatchDataBuilder.MatchOptionsToResponseModel(
                sportOptions.ResponseData,
                courts.ResponseData,
                venues.ResponseData);

            return ServiceResponse<MatchOptionsResponseModel>.Success(response);
        }



        public async Task<ServiceResponse<MatchDetailsResponseModel>>
            GetDetailsAsync(Guid matchId, Guid? userId, bool isAdmin)
        {
            var row = await matchRepository
                .Query()
                .AsSplitQuery()
                .Where(x => x.Id == matchId && !x.IsDeleted && x.Event != null)
                .Select(MatchDataBuilder.MatchDetailsRowProjection())
                .SingleOrDefaultAsync();

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

            var hasEnded = row.Date
                .ToDateTime(row.StartsAt)
                .AddMinutes(row.DurationMinutes) <= timeProvider.GetLocalNow().LocalDateTime;

            var response = MatchDataBuilder.MatchDetailsRowToResponseModel(
                row,
                userId,
                isOrganiser || isAdmin,
                IsOpenForChanges(row.Status, row.Date, row.StartsAt),
                hasEnded);

            return ServiceResponse<MatchDetailsResponseModel>
                .Success(response);
        }

        public async Task<ServiceResponse<MatchEditResponseModel>>
            GetForEditAsync(
                Guid matchId,
                Guid userId,
                bool userIsAdmin)
        {

            var row = await matchRepository
                .Query()
                .Where(x => x.Id == matchId && x.Event != null && x.IsDeleted == false)
                .Select(MatchDataBuilder.MatchEditRowProjection())
                .SingleOrDefaultAsync();

            if (row == null || row.IsDeleted)
            {
                return ServiceResponse<MatchEditResponseModel>
                    .NotFound();
            }

            if (row.OrganiserId != userId && !userIsAdmin)
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

        public async Task<ServiceResponse<Guid>>
            UpdateAsync(MatchUpdateRequestData request)
        {
            var errors = ServiceRequestDataValidator.Validate(request);

            if (errors.Any())
            {
                return ServiceResponse<Guid>.Failure(errors);
            }

            var row = await matchRepository
                .Query()
                .Where(x => x.Id == request.MatchId && x.Event != null)
                .Select(MatchDataBuilder.MatchEditRowProjection())
                .SingleOrDefaultAsync();

            if (row == null || row.IsDeleted)
            {
                return ServiceResponse<Guid>.NotFound();
            }

            if (row.OrganiserId != request.UserId && !request.UserIsAdmin)
            {
                return ServiceResponse<Guid>.Forbidden();
            }

            if (!IsOpenForChanges(row.Status, row.Date, row.StartsAt))
            {
                return ServiceResponse<Guid>.Conflict(
                    "This match is cancelled, finished or already started, so it can no longer be changed.");
            }


            if (request.MaxPlayers < row!.JoinedCount)
            {
                return ServiceResponse<Guid>.Failure(
                    $"{row.JoinedCount} players have already joined, so the maximum cannot be lower.",
                    nameof(request.MaxPlayers));
            }

            if (request.MaxPlayers > row.CourtCapacity)
            {
                return ServiceResponse<Guid>.Failure(
                    $"This court fits at most {row.CourtCapacity} players.", nameof(request.MaxPlayers));
            }

            var match = await matchRepository
                .Query(asNoTracking: false)
                .SingleAsync(x => x.Id == row.Id);

            MatchDataBuilder.MatchUpdateApplyToEntity(match, request);

            matchRepository.Update(match);
            await matchRepository.SaveChangesAsync();

            return ServiceResponse<Guid>.Success(request.MatchId);
        }

        public async Task<ServiceResponse<Guid>>
            CreateAsync(
                MatchCreateRequestData request)
        {
            var errors = ServiceRequestDataValidator.Validate(request);

            if (errors.Any())
            {
                return ServiceResponse<Guid>.Failure(errors);
            }

            if (!await sportRepository.Query().AnyAsync(x => x.Id == request.SportId))
            {
                return ServiceResponse<Guid>.Failure("Select a valid sport.", nameof(request.SportId));
            }

            var court = await courtRepository
                .Query()
                .Where(x => x.Id == request.CourtId)
                .Select(MatchDataBuilder.MatchCourtOptionProjection())
                .SingleOrDefaultAsync();

            if (court == null)
            {
                return ServiceResponse<Guid>
                    .Failure(
                        "Select a valid court.",
                        nameof(request.CourtId));
            }

            if (!court.SportIds.Contains(request.SportId))
            {
                return ServiceResponse<Guid>
                    .Failure(
                        "This court cannot be used for the selected sport.",
                        nameof(request.CourtId));
            }

            if (request.MaxPlayers > court.Capacity)
            {
                return ServiceResponse<Guid>
                    .Failure(
                        $"This court fits at most {court.Capacity} players.",
                        nameof(request.MaxPlayers));
            }

            var slot = await venueRepository
                .Query()
                .Where(x => x.Id == request.VenueId)
                .Select(MatchDataBuilder.SlotRowProjection())
                .SingleOrDefaultAsync();

            if (slot == null || slot.IsDeleted)
            {
                return ServiceResponse<Guid>
                    .Failure(
                        "Select a valid date and time slot.",
                        nameof(request.VenueId));
            }

            if (slot.IsTaken)
            {
                return ServiceResponse<Guid>
                    .Failure(
                        "This slot is already taken.",
                        nameof(request.VenueId));
            }

            if (HasStarted(slot.Date, slot.StartsAt))
            {
                return ServiceResponse<Guid>
                    .Failure(
                        "This slot is in the past.",
                        nameof(request.VenueId));
            }

            var match = MatchDataBuilder.MatchCreateToEntity(
                request,
                timeProvider.GetLocalNow().LocalDateTime);

            await matchRepository.AddAsync(match);
            await matchRepository.SaveChangesAsync();

            return ServiceResponse<Guid>
                .Success(match.Id);
        }


        public async Task<ServiceResponse<MatchCancelResponseModel>>
            GetForCancelAsync(
                Guid matchId,
                Guid userId,
                bool userIsAdmin)
        {
            var match = matchRepository
                .Query()
                .AsSplitQuery()
                .Include(x => x.Participants)
                .Include(x => x.Event)
                    .ThenInclude(x => x!.Venue)
                .Where(x => x.Id == matchId)
                .Where(x => x.Event != null)
                .Where(x => x.IsDeleted == false)
                .SingleOrDefault();

            if (match == null)
            {
                return ServiceResponse<MatchCancelResponseModel>
                    .NotFound();
            }

            if (match.OrganiserId != userId && !userIsAdmin)
            {
                return ServiceResponse<MatchCancelResponseModel>
                    .Forbidden();
            }

            if (match.Status != MatchStatus.Confirmed && match.Status != MatchStatus.Open)
            {
                return ServiceResponse<MatchCancelResponseModel>
                    .Forbidden();
            }

            DateOnly today = DateOnly.FromDateTime(timeProvider.GetLocalNow().LocalDateTime);

            var cancelAllowedDate = today.AddDays(-3);

            if (match.Event!.Venue.Date < cancelAllowedDate)
            {
                return ServiceResponse<MatchCancelResponseModel>
                    .Forbidden();
            }

            var result = new MatchCancelResponseModel
            {
                Title = match.Title,
                StartsAt = match.Event!.Venue.StartsAt,
                Date = match.Event!.Venue.Date,
                VenueName = match.Event!.Venue.Name,
                JoinedCount = match.Participants.Count(x => x.Status == ParticipantStatus.Joined),
            };


            return ServiceResponse<MatchCancelResponseModel>.Success(result);
        }


        public async Task<ServiceResponse<Guid>>
            CancelAsync(MatchCancelRequestData request)
        {
            var errors = ServiceRequestDataValidator.Validate(request);

            if (errors.Any())
            {
                return ServiceResponse<Guid>.Failure(errors);
            }

            var row = await matchRepository
                .Query()
                .Where(x => x.Id == request.MatchId && x.Event != null)
                .Select(MatchDataBuilder.MatchEditRowProjection())
                .SingleOrDefaultAsync();

            if (row == null || row.IsDeleted)
            {
                return ServiceResponse<Guid>.NotFound();
            }

            if (row.OrganiserId != request.UserId && !request.UserIsAdmin)
            {
                return ServiceResponse<Guid>
                    .Forbidden();
            }

            if (!IsOpenForChanges(row.Status, row.Date, row.StartsAt))
            {
                return ServiceResponse<Guid>
                    .Conflict("This match is cancelled, finished or already started, so it can no longer be changed.");
            }

            var match = await matchRepository
                .Query(asNoTracking: false)
                .SingleAsync(x => x.Id == row!.Id);

            MatchDataBuilder.MatchCancelApplyToEntity(match, request);

            await matchRepository.SaveChangesAsync();

            return ServiceResponse<Guid>.Success(match.Id);
        }


        private bool IsOpenForChanges(MatchStatus status, DateOnly date, TimeOnly startsAt)
        {
            return (status == MatchStatus.Open || status == MatchStatus.Confirmed)
                && !HasStarted(date, startsAt);
        }

        private bool HasStarted(DateOnly date, TimeOnly startsAt)
        {
            return date.ToDateTime(startsAt) <= timeProvider.GetLocalNow().LocalDateTime;
        }
    }
}
