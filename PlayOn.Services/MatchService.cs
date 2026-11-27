using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Enums;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Data.RepositoryContract;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Match.RequestData;
using PlayOn.Services.Data.Match.ResponseModels;
using PlayOn.Services.DataBuilders;

namespace PlayOn.Services
{
    /// <summary>
    /// Implements match search, viewing, creation, editing and cancellation, including organiser permission and time-lock rules.
    /// </summary>
    public class MatchService : IMatchService
    {
        private readonly IRepository<Match> matchRepository;
        private readonly IRepository<Sport> sportRepository;
        private readonly IRepository<Court> courtRepository;
        private readonly IRepository<Venue> venueRepository;
        private readonly IRepository<MatchParticipant> participantRepository;
        private readonly IRepository<Notification> notificationRepository;
        private readonly TimeProvider timeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchService"/> class.
        /// </summary>
        /// <param name="matchRepository">Repository for matches.</param>
        /// <param name="sportRepository">Repository for sports.</param>
        /// <param name="courtRepository">Repository for courts.</param>
        /// <param name="venueRepository">Repository for venues.</param>
        /// <param name="participantRepository">Repository for match participants.</param>
        /// <param name="notificationRepository">Repository for notifications.</param>
        /// <param name="timeProvider">Provider of the current time.</param>
        public MatchService(
            IRepository<Match> matchRepository,
            IRepository<Sport> sportRepository,
            IRepository<Court> courtRepository,
            IRepository<Venue> venueRepository,
            IRepository<MatchParticipant> participantRepository,
            IRepository<Notification> notificationRepository,
            TimeProvider timeProvider)
        {
            this.matchRepository = matchRepository;
            this.sportRepository = sportRepository;
            this.courtRepository = courtRepository;
            this.venueRepository = venueRepository;
            this.participantRepository = participantRepository;
            this.notificationRepository = notificationRepository;
            this.timeProvider = timeProvider;
        }

        /// <summary>
        /// Validates the search request and returns one page of matches together with the sport, court and venue options.
        /// </summary>
        /// <param name="request">The search filters.</param>
        /// <param name="pageRequestData">The paging parameters.</param>
        /// <returns>A response with the page and options, or the first failure encountered.</returns>
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

        /// <summary>
        /// Returns a page of upcoming, undeleted open or confirmed matches filtered by sport, city, search term, skill and free spots, and sorted as requested. Page size is limited to 1-50 and the page number is clamped to the valid range.
        /// </summary>
        /// <param name="searchRequestData">The search filters and sort order.</param>
        /// <param name="pageRequestData">The paging parameters.</param>
        /// <returns>A response with the page of match cards, or a failure for invalid filters or an unknown sport.</returns>
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
                && !await sportRepository.Query().AnyAsync(x => x.Id == searchRequestData.SportId.Value))
            {
                return ServiceResponse<PageResult<MatchCardResponseModel>>
                    .Failure("Select a valid sport.", nameof(searchRequestData.SportId));
            }

            var from = searchRequestData.From
                ?? DateOnly.FromDateTime(timeProvider.GetLocalNow().LocalDateTime);

            var query = matchRepository
                .Query()
                .AsSplitQuery()
                .Where(x => !x.IsDeleted)
                .Where(x => x.Status == MatchStatus.Open || x.Status == MatchStatus.Confirmed)
                .Where(x => x.Event != null && !x.Event.Venue.IsDeleted)
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
                    .OrderByDescending(x =>
                        x.MaxPlayers - x.Participants.Count(p => p.Status == ParticipantStatus.Joined))
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

        /// <summary>
        /// Gets all sports as options ordered by name.
        /// </summary>
        /// <returns>A response with the sport options.</returns>
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

        /// <summary>
        /// Gets undeleted, unbooked venue slots dated at least three days ahead, or from the given date if later.
        /// </summary>
        /// <param name="from">The earliest date wanted, or null for the minimum allowed date.</param>
        /// <returns>A response with the venue slot options ordered by date and start time.</returns>
        public async Task<ServiceResponse<IReadOnlyList<VenueOptionResponseModel>>>
            GetVenueOptionsAsync(DateOnly? from)
        {
            var today = DateOnly.FromDateTime(timeProvider.GetLocalNow().LocalDateTime);
            var earliestAllowedDate = today.AddDays(3);
            var fromDate = from.HasValue && from.Value > earliestAllowedDate
                ? from.Value
                : earliestAllowedDate;

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

        /// <summary>
        /// Gets all courts, with their supported sports, as options ordered by name.
        /// </summary>
        /// <returns>A response with the court options.</returns>
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

        /// <summary>
        /// Gets the sport, court and venue options together, stopping at the first failure.
        /// </summary>
        /// <param name="from">The earliest venue date wanted, or null for the default.</param>
        /// <returns>A response with the combined options, or the first failure.</returns>
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

        /// <summary>
        /// Gets the details of an undeleted match, including participants and which actions (manage, join, leave, take attendance, rate) are available to the viewer.
        /// </summary>
        /// <param name="matchId">The identifier of the match.</param>
        /// <param name="userId">The identifier of the viewing user, or null for anonymous.</param>
        /// <param name="isAdmin">Whether the viewer is an administrator.</param>
        /// <returns>A response with the match details, or not-found if the match does not exist or is deleted.</returns>
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
                return ServiceResponse<MatchDetailsResponseModel>.NotFound();
            }

            var isOrganiser = userId != null && row.OrganiserId == userId;

            var hasEnded = row.Date
                .ToDateTime(row.StartsAt)
                .AddMinutes(row.DurationMinutes) <= timeProvider.GetLocalNow().LocalDateTime;

            var response = MatchDataBuilder.MatchDetailsRowToResponseModel(
                row,
                userId,
                isOrganiser || isAdmin,
                IsOpenForChanges(row.Status, row.Date),
                hasEnded);

            return ServiceResponse<MatchDetailsResponseModel>.Success(response);
        }

        /// <summary>
        /// Gets the editable data of a match for its organiser or an administrator.
        /// </summary>
        /// <param name="matchId">The identifier of the match.</param>
        /// <param name="userId">The identifier of the requesting user.</param>
        /// <param name="userIsAdmin">Whether the requesting user is an administrator.</param>
        /// <returns>A response with the edit model; not-found, forbidden, or time-locked (carrying the model) when the match can no longer be changed.</returns>
        public async Task<ServiceResponse<MatchEditResponseModel>>
            GetForEditAsync(Guid matchId, Guid userId, bool userIsAdmin)
        {
            var row = await matchRepository
                .Query()
                .Where(x => x.Id == matchId && x.Event != null && !x.IsDeleted)
                .Select(MatchDataBuilder.MatchEditRowProjection())
                .SingleOrDefaultAsync();

            if (row == null)
            {
                return ServiceResponse<MatchEditResponseModel>.NotFound();
            }

            if (row.OrganiserId != userId && !userIsAdmin)
            {
                return ServiceResponse<MatchEditResponseModel>.Forbidden();
            }

            var responseModel = MatchDataBuilder.MatchEditRowToResponseModel(row);

            if (!IsOpenForChanges(row.Status, row.EditRowDisplayData.Date))
            {
                return ServiceResponse<MatchEditResponseModel>.TimeLocked(responseModel);
            }

            return ServiceResponse<MatchEditResponseModel>.Success(responseModel);
        }

        /// <summary>
        /// Gets the read-only display data of a match for its organiser or an administrator.
        /// </summary>
        /// <param name="matchId">The identifier of the match.</param>
        /// <param name="userId">The identifier of the requesting user.</param>
        /// <param name="userIsAdmin">Whether the requesting user is an administrator.</param>
        /// <returns>A response with the display data, or a not-found or forbidden result.</returns>
        public async Task<ServiceResponse<MatchEditDisplayDataResponseModel>>
            GetEditDisplayDataAsync(
                Guid matchId,
                Guid userId,
                bool userIsAdmin)
        {
            var row = await matchRepository
                .Query()
                .Where(x => x.Id == matchId && x.Event != null && !x.IsDeleted)
                .Select(MatchDataBuilder.MatchEditRowProjection())
                .SingleOrDefaultAsync();

            if (row == null)
            {
                return ServiceResponse<MatchEditDisplayDataResponseModel>.NotFound();
            }

            if (row.OrganiserId != userId && !userIsAdmin)
            {
                return ServiceResponse<MatchEditDisplayDataResponseModel>.Forbidden();
            }

            var displayData = MatchDataBuilder.MatchEditRowToDisplayDataResponseModel(row);

            return ServiceResponse<MatchEditDisplayDataResponseModel>.Success(displayData);
        }

        /// <summary>
        /// Updates a match after validating permissions, the time lock, and that the maximum players is not below the joined count nor above the court capacity. Uses the supplied row version for optimistic concurrency.
        /// </summary>
        /// <param name="request">The updated match data.</param>
        /// <returns>A response with the match identifier on success; validation failure, not-found, forbidden or time-locked otherwise, or conflict if the match was modified concurrently.</returns>
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
                .Where(x => x.Id == request.MatchId && x.Event != null && !x.IsDeleted)
                .Select(MatchDataBuilder.MatchEditRowProjection())
                .SingleOrDefaultAsync();

            if (row == null)
            {
                return ServiceResponse<Guid>.NotFound();
            }

            if (row.OrganiserId != request.UserId && !request.UserIsAdmin)
            {
                return ServiceResponse<Guid>.Forbidden();
            }

            if (!IsOpenForChanges(row.Status, row.EditRowDisplayData.Date))
            {
                return ServiceResponse<Guid>.TimeLocked(row.Id);
            }

            if (request.MaxPlayers < row.EditRowDisplayData.JoinedCount)
            {
                return ServiceResponse<Guid>.Failure(
                    $"{row.EditRowDisplayData.JoinedCount} players have already joined, so the maximum cannot be lower.",
                    nameof(request.MaxPlayers));
            }

            if (request.MaxPlayers > row.EditRowDisplayData.CourtCapacity)
            {
                return ServiceResponse<Guid>.Failure(
                    $"This court fits at most {row.EditRowDisplayData.CourtCapacity} players.",
                    nameof(request.MaxPlayers));
            }

            var match = await matchRepository
                .Query(asNoTracking: false)
                .SingleAsync(x => x.Id == row.Id);

            MatchDataBuilder.MatchUpdateApplyToEntity(match, request);

            matchRepository.SetOriginalPropertyValue(
                match,
                x => x.RowVersion,
                request.RowVersion);

            try
            {
                await matchRepository.SaveChangesAsync();
                return ServiceResponse<Guid>.Success(request.MatchId);
            }
            catch (DbUpdateConcurrencyException)
            {
                return ServiceResponse<Guid>.Conflict();
            }
        }

        /// <summary>
        /// Creates a match after validating that the sport, court and slot exist, the court supports the sport and fits the maximum players, and the slot is free and not time-locked. The organiser joins automatically.
        /// </summary>
        /// <param name="request">The new match data.</param>
        /// <returns>A response with the new match identifier; a validation failure for invalid references or limits, or conflict if the slot was just taken.</returns>
        public async Task<ServiceResponse<Guid>>
            CreateAsync(MatchCreateRequestData request)
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
                return ServiceResponse<Guid>.Failure("Select a valid court.", nameof(request.CourtId));
            }

            if (!court.SportIds.Contains(request.SportId))
            {
                return ServiceResponse<Guid>.Failure(
                    "This court cannot be used for the selected sport.",
                    nameof(request.CourtId));
            }

            if (request.MaxPlayers > court.Capacity)
            {
                return ServiceResponse<Guid>.Failure(
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
                return ServiceResponse<Guid>.Failure(
                    "Select a valid date and time slot.",
                    nameof(request.VenueId));
            }

            if (slot.IsTaken)
            {
                return ServiceResponse<Guid>.Failure(
                    "This slot is already taken.",
                    nameof(request.VenueId));
            }

            if (IsTimeLocked(slot.Date))
            {
                return ServiceResponse<Guid>.Failure(
                    "This slot is too close or in the past.",
                    nameof(request.VenueId));
            }

            var match = MatchDataBuilder.MatchCreateToEntity(
                request,
                timeProvider.GetLocalNow().LocalDateTime);

            await matchRepository.AddAsync(match);

            try
            {
                await matchRepository.SaveChangesAsync();
                return ServiceResponse<Guid>.Success(match.Id);
            }
            catch (DbUpdateException)
            {
                return ServiceResponse<Guid>.Conflict("This slot was just taken by another booking.");
            }
        }

        /// <summary>
        /// Gets the summary for the cancel confirmation page of an open or confirmed match.
        /// </summary>
        /// <param name="matchId">The identifier of the match.</param>
        /// <param name="userId">The identifier of the requesting user.</param>
        /// <param name="userIsAdmin">Whether the requesting user is an administrator.</param>
        /// <returns>A response with the cancellation summary; not-found, forbidden (including a match that is not open or confirmed), or time-locked (carrying the summary).</returns>
        public async Task<ServiceResponse<MatchCancelResponseModel>>
            GetForCancelAsync(Guid matchId, Guid userId, bool userIsAdmin)
        {
            var match = await matchRepository
                .Query()
                .AsSplitQuery()
                .Include(x => x.Participants)
                .Include(x => x.Event)
                    .ThenInclude(x => x!.Venue)
                .Where(x => x.Id == matchId && x.Event != null && !x.IsDeleted)
                .SingleOrDefaultAsync();

            if (match == null)
            {
                return ServiceResponse<MatchCancelResponseModel>.NotFound();
            }

            if (match.OrganiserId != userId && !userIsAdmin)
            {
                return ServiceResponse<MatchCancelResponseModel>.Forbidden();
            }

            if (match.Status != MatchStatus.Confirmed && match.Status != MatchStatus.Open)
            {
                return ServiceResponse<MatchCancelResponseModel>.Forbidden();
            }

            if (IsTimeLocked(match.Event!.Venue.Date))
            {
                return ServiceResponse<MatchCancelResponseModel>.TimeLocked(
                    new MatchCancelResponseModel
                    {
                        Title = match.Title,
                        StartsAt = match.Event.Venue.StartsAt,
                        Date = match.Event.Venue.Date,
                        VenueName = match.Event.Venue.Name,
                        JoinedCount = match.Participants.Count(x => x.Status == ParticipantStatus.Joined),
                    });
            }

            var result = new MatchCancelResponseModel
            {
                Title = match.Title,
                StartsAt = match.Event.Venue.StartsAt,
                Date = match.Event.Venue.Date,
                VenueName = match.Event.Venue.Name,
                JoinedCount = match.Participants.Count(x => x.Status == ParticipantStatus.Joined),
            };

            return ServiceResponse<MatchCancelResponseModel>.Success(result);
        }

        /// <summary>
        /// Cancels an open or confirmed match, stores the reason and notifies every other joined or waitlisted participant.
        /// </summary>
        /// <param name="request">The cancellation data, including the reason.</param>
        /// <returns>A response with the match identifier on success; validation failure, not-found, forbidden, time-locked or conflict (match not open or concurrent change) otherwise.</returns>
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
                .Where(x => x.Id == request.MatchId && x.Event != null && !x.IsDeleted)
                .Select(MatchDataBuilder.MatchEditRowProjection())
                .SingleOrDefaultAsync();

            if (row == null)
            {
                return ServiceResponse<Guid>.NotFound();
            }

            if (row.OrganiserId != request.UserId && !request.UserIsAdmin)
            {
                return ServiceResponse<Guid>.Forbidden();
            }

            if (IsTimeLocked(row.EditRowDisplayData.Date))
            {
                return ServiceResponse<Guid>.TimeLocked(row.Id);
            }

            if (row.Status != MatchStatus.Open && row.Status != MatchStatus.Confirmed)
            {
                return ServiceResponse<Guid>.Conflict(
                    "This match is cancelled, finished or already started, so it can no longer be changed.");
            }

            var match = await matchRepository
                .Query(asNoTracking: false)
                .SingleAsync(x => x.Id == row.Id);

            MatchDataBuilder.MatchCancelApplyToEntity(match, request);

            var localDateTime = timeProvider.GetLocalNow().LocalDateTime;
            var reason = request.Reason.Trim();

            var recipientIds = await participantRepository
                .Query()
                .Where(x => x.MatchId == match.Id
                    && x.UserId != request.UserId
                    && (x.Status == ParticipantStatus.Joined
                        || x.Status == ParticipantStatus.Waitlisted))
                .Select(x => x.UserId)
                .ToListAsync();

            if (recipientIds.Count > 0)
            {
                var notifications = recipientIds
                    .Select(userId => NotificationDataBuilder.NotificationToEntity(
                        userId,
                        NotificationType.MatchCancelled,
                        $"\"{match.Title}\" was cancelled: {reason}",
                        match.Id,
                        localDateTime))
                    .ToList();

                await notificationRepository.AddRangeAsync(notifications);
            }

            try
            {
                await matchRepository.SaveChangesAsync();
                return ServiceResponse<Guid>.Success(match.Id);
            }
            catch (DbUpdateConcurrencyException)
            {
                return ServiceResponse<Guid>.Conflict();
            }
        }

        /// <summary>
        /// Determines whether a match can still be changed: it must be open or confirmed and not time-locked.
        /// </summary>
        /// <param name="status">The current match status.</param>
        /// <param name="date">The date of the match.</param>
        /// <returns>True if the match can be changed; otherwise false.</returns>
        private bool IsOpenForChanges(MatchStatus status, DateOnly date)
        {
            return (status == MatchStatus.Open || status == MatchStatus.Confirmed)
                && !IsTimeLocked(date);
        }

        /// <summary>
        /// Determines whether a date is within the three-day lock window before the match, or already past.
        /// </summary>
        /// <param name="date">The date of the match.</param>
        /// <returns>True if the date is less than three days from today; otherwise false.</returns>
        private bool IsTimeLocked(DateOnly date)
        {
            DateOnly today = DateOnly.FromDateTime(timeProvider.GetLocalNow().LocalDateTime);

            return date < today.AddDays(3);
        }
    }
}