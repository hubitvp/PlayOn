using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Enums;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.AdminVenues.RequestData;
using PlayOn.Services.Data.AdminVenues.ResponseModels;
using PlayOn.Services.DataBuilders;

namespace PlayOn.Services
{
    public class AdminVenueService : IAdminVenueService
    {
        private readonly IRepository<Venue> venueRepository;
        private readonly TimeProvider timeProvider;

        public AdminVenueService(
            IRepository<Venue> venueRepository,
            TimeProvider timeProvider)
        {
            this.venueRepository = venueRepository;
            this.timeProvider = timeProvider;
        }

        public async Task<ServiceResponse<PageResult<AdminVenueResponseModel>>>
            GetVenuesAsync(AdminVenueQueryRequestData request)
        {
            var errors = ServiceRequestDataValidator.Validate(request);

            if (errors.Any())
            {
                return ServiceResponse<PageResult<AdminVenueResponseModel>>.Failure(errors);
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

        public async Task<ServiceResponse<AdminVenueEditResponseModel>>
            GetForEditAsync(Guid venueId)
        {
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

        public async Task<ServiceResponse<Guid>>
            CreateAsync(AdminVenueCreateRequestData request)
        {
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

        public async Task<ServiceResponse<Guid>>
            UpdateAsync(AdminVenueUpdateRequestData request)
        {
            var errors = ServiceRequestDataValidator.Validate(request);

            if (errors.Any())
            {
                return ServiceResponse<Guid>.Failure(errors);
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

        public async Task<ServiceResponse<Guid>>
            DeleteAsync(Guid venueId)
        {
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

        public async Task<ServiceResponse<Guid>>
            RestoreAsync(Guid venueId)
        {
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

        private DateOnly Today()
        {
            return DateOnly.FromDateTime(timeProvider.GetLocalNow().DateTime);
        }

        private Task<bool> IsBookedAsync(Guid venueId)
        {
            return venueRepository
                .Query()
                .AnyAsync(x => x.Id == venueId && x.Event != null);
        }

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
