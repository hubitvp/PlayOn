using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Enums;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Data.RepositoryContract;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.AdminVenues.RequestData;
using PlayOn.Services.Data.AdminVenues.ResponseModels;
using PlayOn.Services.DataBuilders;

namespace PlayOn.Services
{
    /// <summary>
    /// Implements administrative management operations for venue slots.
    /// </summary>
    public class AdminVenueService : IAdminVenueService
    {
        private readonly IRepository<Venue> venueRepository;
        private readonly TimeProvider timeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminVenueService"/> class.
        /// </summary>
        /// <param name="venueRepository">The venue repository.</param>
        /// <param name="timeProvider">The time provider used to determine today's date.</param>
        public AdminVenueService(
            IRepository<Venue> venueRepository,
            TimeProvider timeProvider)
        {
            this.venueRepository = venueRepository;
            this.timeProvider = timeProvider;
        }

        /// <summary>
        /// Gets venue slots filtered by an optional search term (name, city or address) and a free, booked or deleted status, ordered by date and start time.
        /// </summary>
        /// <param name="request">The search, status filter and paging options.</param>
        /// <returns>A page of venues, or a failure response when the request or status filter is invalid.</returns>
        public async Task<ServiceResponse<PageResult<AdminVenueResponseModel>>>
            GetVenuesAsync(AdminVenueQueryRequestData request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var errors = ServiceRequestDataValidator.Validate(request);
            if (errors.Any())
            {
                return ServiceResponse<PageResult<AdminVenueResponseModel>>.Failure(errors);
            }

            if (!Enum.IsDefined(request.Status))
            {
                return ServiceResponse<PageResult<AdminVenueResponseModel>>.Failure(
                    "Select a valid status filter.",
                    nameof(request.Status));
            }

            var query = venueRepository.Query();

            var term = request.Search?.Trim();
            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(x =>
                    x.Name.Contains(term)
                    || x.City.Contains(term)
                    || x.Address.Contains(term));
            }

            query = request.Status switch
            {
                VenueStatusFilter.Free => query.Where(x => !x.IsDeleted && x.Event == null),
                VenueStatusFilter.Booked => query.Where(x => !x.IsDeleted && x.Event != null),
                VenueStatusFilter.Deleted => query.Where(x => x.IsDeleted),
                _ => query
            };

            var totalCount = await query.CountAsync();

            var pageSize = Math.Clamp(request.PageSize, 1, 50);
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize));
            var pageNumber = Math.Clamp(request.Page, 1, totalPages);

            var rows = await query
                .OrderBy(x => x.Date)
                .ThenBy(x => x.StartsAt)
                .ThenBy(x => x.Name)
                .ThenBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(AdminVenueDataBuilder.VenueProjection())
                .ToListAsync();

            var items = rows
                .Select(AdminVenueDataBuilder.VenueToResponseModel)
                .ToList();

            var page = new PageResult<AdminVenueResponseModel>(items, pageNumber, pageSize, totalCount);

            return ServiceResponse<PageResult<AdminVenueResponseModel>>.Success(page);
        }

        /// <summary>
        /// Gets the edit model of a venue slot, including whether it is booked.
        /// </summary>
        /// <param name="venueId">The identifier of the venue slot.</param>
        /// <returns>The edit model, or a not-found response when the identifier is empty or the venue does not exist.</returns>
        public async Task<ServiceResponse<AdminVenueEditResponseModel>>
            GetForEditAsync(Guid venueId)
        {
            if (venueId == Guid.Empty)
            {
                return ServiceResponse<AdminVenueEditResponseModel>.NotFound();
            }

            var venue = await venueRepository
                .Query()
                .SingleOrDefaultAsync(x => x.Id == venueId);

            if (venue == null)
            {
                return ServiceResponse<AdminVenueEditResponseModel>.NotFound();
            }

            var isBooked = await IsBookedAsync(venueId);

            var model = AdminVenueDataBuilder.VenueToEditResponseModel(venue, isBooked);

            return ServiceResponse<AdminVenueEditResponseModel>.Success(model);
        }

        /// <summary>
        /// Creates a venue slot after validating the request, rejecting past dates and duplicate slots.
        /// </summary>
        /// <param name="request">The data of the venue slot to create.</param>
        /// <returns>The identifier of the created venue, or a failure response when validation fails, the date is in the past or the slot already exists.</returns>
        public async Task<ServiceResponse<Guid>>
            CreateAsync(AdminVenueCreateRequestData request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var errors = ServiceRequestDataValidator.Validate(request);
            if (errors.Any())
            {
                return ServiceResponse<Guid>.Failure(errors);
            }

            if (request.Date < Today())
            {
                return ServiceResponse<Guid>.Failure(
                    "The date cannot be in the past.",
                    nameof(request.Date));
            }

            if (await SlotExistsAsync(request, excludedVenueId: null))
            {
                return ServiceResponse<Guid>.Failure(
                    "This slot already exists for this venue.",
                    nameof(request.StartsAt));
            }

            var venue = AdminVenueDataBuilder.ToEntity(request);

            await venueRepository.AddAsync(venue);
            await venueRepository.SaveChangesAsync();

            return ServiceResponse<Guid>.Success(venue.Id);
        }

        /// <summary>
        /// Updates a venue slot. Deleted slots cannot be edited, and the date or start time cannot change while a match is booked.
        /// </summary>
        /// <param name="request">The updated venue data, including the venue identifier.</param>
        /// <returns>The venue identifier, a not-found response, a conflict response for deleted or booked slots, or a failure response for validation errors, a past date or a duplicate slot.</returns>
        public async Task<ServiceResponse<Guid>>
            UpdateAsync(AdminVenueUpdateRequestData request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var errors = ServiceRequestDataValidator.Validate(request);
            if (errors.Any())
            {
                return ServiceResponse<Guid>.Failure(errors);
            }

            if (request.VenueId == Guid.Empty)
            {
                return ServiceResponse<Guid>.NotFound();
            }

            var venue = await venueRepository
                .Query(asNoTracking: false)
                .SingleOrDefaultAsync(x => x.Id == request.VenueId);

            if (venue == null)
            {
                return ServiceResponse<Guid>.NotFound();
            }

            if (venue.IsDeleted)
            {
                return ServiceResponse<Guid>.Conflict("Restore the slot before editing it.");
            }

            var dateChanged = venue.Date != request.Date;
            var startChanged = venue.StartsAt != request.StartsAt;

            if (dateChanged && request.Date < Today())
            {
                return ServiceResponse<Guid>.Failure(
                    "The date cannot be in the past.",
                    nameof(request.Date));
            }

            if ((dateChanged || startChanged) && await IsBookedAsync(venue.Id))
            {
                return ServiceResponse<Guid>.Conflict(
                    "A match is booked on this slot, so its date and start time cannot be changed.");
            }

            if (await SlotExistsAsync(request, request.VenueId))
            {
                return ServiceResponse<Guid>.Failure(
                    "This slot already exists for this venue.",
                    nameof(request.StartsAt));
            }

            AdminVenueDataBuilder.ApplyToEntity(request, venue);

            await venueRepository.SaveChangesAsync();

            return ServiceResponse<Guid>.Success(venue.Id);
        }

        /// <summary>
        /// Soft-deletes a venue slot unless a match is booked on it. Deleting an already deleted slot succeeds.
        /// </summary>
        /// <param name="venueId">The identifier of the venue slot to delete.</param>
        /// <returns>The venue identifier, a not-found response when it does not exist, or a conflict response when it is booked.</returns>
        public async Task<ServiceResponse<Guid>>
            DeleteAsync(Guid venueId)
        {
            if (venueId == Guid.Empty)
            {
                return ServiceResponse<Guid>.NotFound();
            }

            var venue = await venueRepository
                .Query(asNoTracking: false)
                .SingleOrDefaultAsync(x => x.Id == venueId);

            if (venue == null)
            {
                return ServiceResponse<Guid>.NotFound();
            }

            if (venue.IsDeleted)
            {
                return ServiceResponse<Guid>.Success(venue.Id);
            }

            if (await IsBookedAsync(venueId))
            {
                return ServiceResponse<Guid>.Conflict(
                    "A match is booked on this slot, so it cannot be deleted.");
            }

            venue.IsDeleted = true;

            await venueRepository.SaveChangesAsync();

            return ServiceResponse<Guid>.Success(venue.Id);
        }

        /// <summary>
        /// Restores a soft-deleted venue slot. Restoring a slot that is not deleted succeeds.
        /// </summary>
        /// <param name="venueId">The identifier of the venue slot to restore.</param>
        /// <returns>The venue identifier, or a not-found response when it does not exist.</returns>
        public async Task<ServiceResponse<Guid>>
            RestoreAsync(Guid venueId)
        {
            if (venueId == Guid.Empty)
            {
                return ServiceResponse<Guid>.NotFound();
            }

            var venue = await venueRepository
                .Query(asNoTracking: false)
                .SingleOrDefaultAsync(x => x.Id == venueId);

            if (venue == null)
            {
                return ServiceResponse<Guid>.NotFound();
            }

            if (!venue.IsDeleted)
            {
                return ServiceResponse<Guid>.Success(venue.Id);
            }

            venue.IsDeleted = false;

            await venueRepository.SaveChangesAsync();

            return ServiceResponse<Guid>.Success(venue.Id);
        }

        /// <summary>
        /// Gets today's date in the local time zone of the time provider.
        /// </summary>
        /// <returns>The current local date.</returns>
        private DateOnly Today()
        {
            return DateOnly.FromDateTime(timeProvider.GetLocalNow().DateTime);
        }

        /// <summary>
        /// Checks whether a match is booked on the venue slot.
        /// </summary>
        /// <param name="venueId">The identifier of the venue slot.</param>
        /// <returns>True if the slot has an event; otherwise false.</returns>
        private Task<bool> IsBookedAsync(Guid venueId)
        {
            return venueRepository
                .Query()
                .AnyAsync(x => x.Id == venueId && x.Event != null);
        }

        /// <summary>
        /// Checks whether another venue slot with the same trimmed name, city, date and start time exists.
        /// </summary>
        /// <param name="request">The venue data whose slot is checked.</param>
        /// <param name="excludedVenueId">An identifier to exclude from the check, or null to check all venues.</param>
        /// <returns>True if a matching slot exists; otherwise false.</returns>
        private Task<bool> SlotExistsAsync(AdminVenueRequestDataBase request, Guid? excludedVenueId)
        {
            var name = request.Name.Trim();
            var city = request.City.Trim();

            return venueRepository
                .Query()
                .AnyAsync(x =>
                    x.Name == name
                    && x.City == city
                    && x.Date == request.Date
                    && x.StartsAt == request.StartsAt
                    && (excludedVenueId == null || x.Id != excludedVenueId));
        }
    }
}