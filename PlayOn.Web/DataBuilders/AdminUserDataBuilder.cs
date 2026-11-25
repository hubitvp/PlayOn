using PlayOn.Common.Helpers;
using PlayOn.Services.Data.AdminUsers.RequestData;
using PlayOn.Services.Data.AdminUsers.ResponseModels;
using PlayOn.Web.ViewModels.Admin;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Web.DataBuilders;

public class AdminUserDataBuilder : DataBuilder
{
    public AdminUserQueryRequestData
        UserQueryToRequestData(AdminUserSearchInputModel input)
    {
        var result = new AdminUserQueryRequestData
        {
            Search = input.Search,
            Status = input.Status,
            Page = input.Page,
            PageSize = AdminPageSize
        };

        return result;
    }

    public AdminUserStatusRequestData
        UserStatusToRequestData(Guid userId, Guid actorId, bool isActive)
    {
        var result = new AdminUserStatusRequestData
        {
            UserId = userId,
            ActorId = actorId,
            IsActive = isActive
        };

        return result;
    }

    public AdminUserRoleRequestData
        UserRoleToRequestData(Guid userId, Guid actorId, bool isAdministrator)
    {
        var result = new AdminUserRoleRequestData
        {
            UserId = userId,
            ActorId = actorId,
            IsAdministrator = isAdministrator
        };

        return result;
    }

    public AdminUserListViewModel
        UserListToViewModel(
            PageResult<AdminUserResponseModel>? page,
            AdminUserSearchInputModel input,
            Guid currentUserId)
    {
        var result = new AdminUserListViewModel
        {
            Users = page?.PageItems
                .Select(x => UserToViewModel(x, currentUserId))
                .ToList() ?? [],
            Search = input.Search,
            Status = input.Status,
            TotalCount = page?.TotalItemsCount ?? 0,
            Page = page?.PageMark ?? input.Page,
            PageSize = page?.PageSize ?? AdminPageSize,
            TotalPages = page?.TotalPages ?? 0
        };

        return result;
    }

    public AdminUserItemViewModel
        UserToViewModel(AdminUserResponseModel source, Guid currentUserId)
    {
        var result = new AdminUserItemViewModel
        {
            Id = source.Id,
            DisplayName = source.DisplayName,
            Email = source.Email ?? string.Empty,
            City = source.City,
            IsActive = source.IsActive,
            IsAdministrator = source.IsAdministrator,
            IsCurrentUser = source.Id == currentUserId,
            MatchesPlayed = source.MatchesPlayed,
            Reliability = source.ReliabilityScore is int score ? $"{score}%" : "-"
        };

        return result;
    }
}
