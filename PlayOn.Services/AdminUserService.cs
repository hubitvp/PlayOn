using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlayOn.Common.ApplicationConstants;
using PlayOn.Common.Enums;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.AdminUsers.RequestData;
using PlayOn.Services.Data.AdminUsers.ResponseModels;
using PlayOn.Services.DataBuilders;

namespace PlayOn.Services
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IRepository<ApplicationUser> userRepository;
        private readonly IRepository<ApplicationRole> roleRepository;
        private readonly IRepository<IdentityUserRole<Guid>> userRoleRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public AdminUserService(
            IRepository<ApplicationUser> userRepository,
            IRepository<ApplicationRole> roleRepository,
            IRepository<IdentityUserRole<Guid>> userRoleRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.userRoleRepository = userRoleRepository;
            this.userManager = userManager;
        }

        public async Task<ServiceResponse<PageResult<AdminUserResponseModel>>>
            GetUsersAsync(AdminUserQueryRequestData request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var errors = ServiceRequestDataValidator.Validate(request);
            if (errors.Any())
            {
                return ServiceResponse<PageResult<AdminUserResponseModel>>.Failure(errors);
            }

            if (!Enum.IsDefined(request.Status))
            {
                return ServiceResponse<PageResult<AdminUserResponseModel>>.Failure(
                    "Select a valid status filter.",
                    nameof(request.Status));
            }

            var query = userRepository.Query();

            var term = request.Search?.Trim();
            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(x =>
                    x.DisplayName.Contains(term)
                    || x.City.Contains(term)
                    || (x.Email != null && x.Email.Contains(term)));
            }

            query = request.Status switch
            {
                UserStatusFilter.Active => query.Where(x => x.IsActive),
                UserStatusFilter.Inactive => query.Where(x => !x.IsActive),
                _ => query
            };

            var totalCount = await query.CountAsync();

            var pageSize = Math.Clamp(request.PageSize, 1, 50);
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize));
            var pageNumber = Math.Clamp(request.Page, 1, totalPages);

            var rows = await query
                .OrderBy(x => x.DisplayName)
                .ThenBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(AdminUserDataBuilder.UserProjection())
                .ToListAsync();

            var administratorIds = await GetAdministratorIdsAsync(rows.Select(x => x.Id).ToList());

            var items = rows
                .Select(x => AdminUserDataBuilder.UserToResponseModel(x, administratorIds.Contains(x.Id)))
                .ToList();

            var page = new PageResult<AdminUserResponseModel>(items, pageNumber, pageSize, totalCount);

            return ServiceResponse<PageResult<AdminUserResponseModel>>.Success(page);
        }

        public async Task<ServiceResponse<Guid>>
            SetActiveAsync(AdminUserStatusRequestData request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var errors = ServiceRequestDataValidator.Validate(request);
            if (errors.Any())
            {
                return ServiceResponse<Guid>.Failure(errors);
            }

            if (request.UserId == Guid.Empty)
            {
                return ServiceResponse<Guid>.NotFound();
            }

            if (!request.IsActive && request.UserId == request.ActorId)
            {
                return ServiceResponse<Guid>.Conflict("You cannot deactivate your own account.");
            }

            var user = await userRepository
                .Query(asNoTracking: false)
                .SingleOrDefaultAsync(x => x.Id == request.UserId);

            if (user == null)
            {
                return ServiceResponse<Guid>.NotFound();
            }

            if (user.IsActive == request.IsActive)
            {
                return ServiceResponse<Guid>.Success(user.Id);
            }

            user.IsActive = request.IsActive;

            await userRepository.SaveChangesAsync();

            if (!request.IsActive)
            {
                // Signs the user out everywhere the next time the cookie is validated.
                await userManager.UpdateSecurityStampAsync(user);
            }

            return ServiceResponse<Guid>.Success(user.Id);
        }

        public async Task<ServiceResponse<Guid>>
            SetAdministratorAsync(AdminUserRoleRequestData request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var errors = ServiceRequestDataValidator.Validate(request);
            if (errors.Any())
            {
                return ServiceResponse<Guid>.Failure(errors);
            }

            if (request.UserId == Guid.Empty)
            {
                return ServiceResponse<Guid>.NotFound();
            }

            if (!request.IsAdministrator && request.UserId == request.ActorId)
            {
                return ServiceResponse<Guid>.Conflict("You cannot remove your own administrator rights.");
            }

            var user = await userManager.FindByIdAsync(request.UserId.ToString());

            if (user == null)
            {
                return ServiceResponse<Guid>.NotFound();
            }

            var isAdministrator = await userManager.IsInRoleAsync(user, RoleNames.Administrator);

            if (isAdministrator == request.IsAdministrator)
            {
                return ServiceResponse<Guid>.Success(user.Id);
            }

            var result = request.IsAdministrator
                ? await userManager.AddToRoleAsync(user, RoleNames.Administrator)
                : await userManager.RemoveFromRoleAsync(user, RoleNames.Administrator);

            if (!result.Succeeded)
            {
                var message = string.Join(" ", result.Errors.Select(x => x.Description));

                return ServiceResponse<Guid>.Conflict(message);
            }

            return ServiceResponse<Guid>.Success(user.Id);
        }

        private async Task<HashSet<Guid>> GetAdministratorIdsAsync(IReadOnlyCollection<Guid> userIds)
        {
            if (userIds.Count == 0)
            {
                return [];
            }

            var administratorRoleId = await roleRepository
                .Query()
                .Where(x => x.Name == RoleNames.Administrator)
                .Select(x => (Guid?)x.Id)
                .SingleOrDefaultAsync();

            if (administratorRoleId == null)
            {
                return [];
            }

            var roleId = administratorRoleId.Value;

            var ids = await userRoleRepository
                .Query()
                .Where(x => x.RoleId == roleId && userIds.Contains(x.UserId))
                .Select(x => x.UserId)
                .ToListAsync();

            return ids.ToHashSet();
        }
    }
}