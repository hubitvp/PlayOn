using Microsoft.EntityFrameworkCore;
using PlayOn.Common.ApplicationConstants;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.AdminSports.RequestData;
using PlayOn.Services.Data.AdminSports.ResponseModels;
using PlayOn.Services.DataBuilders;

namespace PlayOn.Services
{
    public class AdminSportService : IAdminSportService
    {
        private readonly IRepository<Sport> sportRepository;

        public AdminSportService(IRepository<Sport> sportRepository)
        {
            this.sportRepository = sportRepository;
        }

        public async Task<ServiceResponse<PageResult<AdminSportResponseModel>>>
            GetSportsAsync(AdminSportQueryRequestData request)
        {
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

        public async Task<ServiceResponse<AdminSportEditResponseModel>>
            GetForEditAsync(Guid sportId)
        {
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

        public async Task<ServiceResponse<Guid>>
            CreateAsync(AdminSportCreateRequestData request)
        {
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

        public async Task<ServiceResponse<Guid>>
            UpdateAsync(AdminSportUpdateRequestData request)
        {
            var errors = ServiceRequestDataValidator.Validate(request);

            if (errors.Any())
            {
                return ServiceResponse<Guid>.Failure(errors);
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

        public async Task<ServiceResponse<Guid>>
            DeleteAsync(Guid sportId)
        {
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
