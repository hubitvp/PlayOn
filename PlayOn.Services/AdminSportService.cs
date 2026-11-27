using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Data.RepositoryContract;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.AdminSports.RequestData;
using PlayOn.Services.Data.AdminSports.ResponseModels;
using PlayOn.Services.DataBuilders;

namespace PlayOn.Services
{
    /// <summary>
    /// Implements administrative management operations for sports.
    /// </summary>
    public class AdminSportService : IAdminSportService
    {
        private readonly IRepository<Sport> sportRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminSportService"/> class.
        /// </summary>
        /// <param name="sportRepository">The sport repository.</param>
        public AdminSportService(IRepository<Sport> sportRepository)
        {
            this.sportRepository = sportRepository;
        }

        /// <summary>
        /// Gets sports filtered by an optional name search, ordered by name. The page size is clamped to 1-50 and the page number to the available range.
        /// </summary>
        /// <param name="request">The search and paging options.</param>
        /// <returns>A page of sports, or a failure response when the request is invalid.</returns>
        public async Task<ServiceResponse<PageResult<AdminSportResponseModel>>>
            GetSportsAsync(AdminSportQueryRequestData request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var errors = ServiceRequestDataValidator.Validate(request);
            if (errors.Any())
            {
                return ServiceResponse<PageResult<AdminSportResponseModel>>.Failure(errors);
            }

            var query = sportRepository.Query();

            var term = request.Search?.Trim();
            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(x => x.Name.Contains(term));
            }

            var totalCount = await query.CountAsync();

            var pageSize = Math.Clamp(request.PageSize, 1, 50);
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize));
            var pageNumber = Math.Clamp(request.Page, 1, totalPages);

            var rows = await query
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(AdminSportDataBuilder.SportProjection())
                .ToListAsync();

            var items = rows
                .Select(AdminSportDataBuilder.SportToResponseModel)
                .ToList();

            var page = new PageResult<AdminSportResponseModel>(items, pageNumber, pageSize, totalCount);

            return ServiceResponse<PageResult<AdminSportResponseModel>>.Success(page);
        }

        /// <summary>
        /// Gets the edit model of a sport.
        /// </summary>
        /// <param name="sportId">The identifier of the sport.</param>
        /// <returns>The edit model, or a not-found response when the identifier is empty or the sport does not exist.</returns>
        public async Task<ServiceResponse<AdminSportEditResponseModel>>
            GetForEditAsync(Guid sportId)
        {
            if (sportId == Guid.Empty)
            {
                return ServiceResponse<AdminSportEditResponseModel>.NotFound();
            }

            var sport = await sportRepository
                .Query()
                .SingleOrDefaultAsync(x => x.Id == sportId);

            if (sport == null)
            {
                return ServiceResponse<AdminSportEditResponseModel>.NotFound();
            }

            var model = AdminSportDataBuilder.SportToEditResponseModel(sport);

            return ServiceResponse<AdminSportEditResponseModel>.Success(model);
        }

        /// <summary>
        /// Creates a sport after validating the request and checking that no sport has the same name.
        /// </summary>
        /// <param name="request">The data of the sport to create.</param>
        /// <returns>The identifier of the created sport, or a failure response when validation fails or the name already exists.</returns>
        public async Task<ServiceResponse<Guid>>
            CreateAsync(AdminSportCreateRequestData request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var errors = ServiceRequestDataValidator.Validate(request);
            if (errors.Any())
            {
                return ServiceResponse<Guid>.Failure(errors);
            }

            var name = request.Name.Trim();

            var exists = await sportRepository
                .Query()
                .AnyAsync(x => x.Name == name);

            if (exists)
            {
                return ServiceResponse<Guid>.Failure(
                    "A sport with this name already exists.",
                    nameof(request.Name));
            }

            var sport = AdminSportDataBuilder.ToEntity(request);

            await sportRepository.AddAsync(sport);
            await sportRepository.SaveChangesAsync();

            return ServiceResponse<Guid>.Success(sport.Id);
        }

        /// <summary>
        /// Updates a sport after validating the request and checking that no other sport has the same name.
        /// </summary>
        /// <param name="request">The updated sport data, including the sport identifier.</param>
        /// <returns>The identifier of the updated sport, or a not-found response when it does not exist, or a failure response when validation fails or the name is taken.</returns>
        public async Task<ServiceResponse<Guid>>
            UpdateAsync(AdminSportUpdateRequestData request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var errors = ServiceRequestDataValidator.Validate(request);
            if (errors.Any())
            {
                return ServiceResponse<Guid>.Failure(errors);
            }

            if (request.SportId == Guid.Empty)
            {
                return ServiceResponse<Guid>.NotFound();
            }

            var sport = await sportRepository
                .Query(asNoTracking: false)
                .SingleOrDefaultAsync(x => x.Id == request.SportId);

            if (sport == null)
            {
                return ServiceResponse<Guid>.NotFound();
            }

            var name = request.Name.Trim();

            var duplicate = await sportRepository
                .Query()
                .AnyAsync(x => x.Name == name && x.Id != request.SportId);

            if (duplicate)
            {
                return ServiceResponse<Guid>.Failure(
                    "A sport with this name already exists.",
                    nameof(request.Name));
            }

            AdminSportDataBuilder.ApplyToEntity(request, sport);

            await sportRepository.SaveChangesAsync();

            return ServiceResponse<Guid>.Success(sport.Id);
        }

        /// <summary>
        /// Deletes a sport unless it is referenced by courts, matches or player preferences.
        /// </summary>
        /// <param name="sportId">The identifier of the sport to delete.</param>
        /// <returns>The identifier of the deleted sport, a not-found response when it does not exist, or a conflict response when it is in use.</returns>
        public async Task<ServiceResponse<Guid>>
            DeleteAsync(Guid sportId)
        {
            if (sportId == Guid.Empty)
            {
                return ServiceResponse<Guid>.NotFound();
            }

            var sport = await sportRepository
                .Query(asNoTracking: false)
                .SingleOrDefaultAsync(x => x.Id == sportId);

            if (sport == null)
            {
                return ServiceResponse<Guid>.NotFound();
            }

            var isUsed = await sportRepository
                .Query()
                .Where(x => x.Id == sportId)
                .AnyAsync(x => x.Courts.Any() || x.Events.Any() || x.Preferences.Any());

            if (isUsed)
            {
                return ServiceResponse<Guid>.Conflict(
                    "This sport is used by courts, matches or player preferences and cannot be deleted.");
            }

            sportRepository.Remove(sport);
            await sportRepository.SaveChangesAsync();

            return ServiceResponse<Guid>.Success(sportId);
        }
    }
}