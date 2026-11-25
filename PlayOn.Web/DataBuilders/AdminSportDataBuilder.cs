using PlayOn.Common.Helpers;
using PlayOn.Services.Data.AdminSports.RequestData;
using PlayOn.Services.Data.AdminSports.ResponseModels;
using PlayOn.Web.ViewModels.Admin;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Web.DataBuilders;

public class AdminSportDataBuilder : DataBuilder
{
    public AdminSportQueryRequestData
        SportQueryToRequestData(AdminSportSearchInputModel input)
    {
        var result = new AdminSportQueryRequestData
        {
            Search = input.Search,
            Page = input.Page,
            PageSize = AdminPageSize
        };

        return result;
    }

    public AdminSportListViewModel
        SportListToViewModel(
            PageResult<AdminSportResponseModel>? page,
            AdminSportSearchInputModel input)
    {
        var result = new AdminSportListViewModel
        {
            Sports = page?.PageItems
                .Select(SportToViewModel)
                .ToList() ?? [],
            Search = input.Search,
            TotalCount = page?.TotalItemsCount ?? 0,
            Page = page?.PageMark ?? input.Page,
            PageSize = page?.PageSize ?? AdminPageSize,
            TotalPages = page?.TotalPages ?? 0
        };

        return result;
    }

    public AdminSportItemViewModel
        SportToViewModel(AdminSportResponseModel source)
    {
        var result = new AdminSportItemViewModel
        {
            Id = source.Id,
            Name = source.Name,
            IconCss = source.Icon.ToCssClass(),
            DefaultMinPlayers = source.DefaultMinPlayers,
            DefaultMaxPlayers = source.DefaultMaxPlayers,
            CourtsCount = source.CourtsCount,
            MatchesCount = source.MatchesCount,
            PreferencesCount = source.PreferencesCount,
            CanDelete = source.CourtsCount == 0
                        && source.MatchesCount == 0
                        && source.PreferencesCount == 0
        };

        return result;
    }

    public AdminSportFormViewModel
        SportToFormViewModel(AdminSportEditResponseModel source)
    {
        var result = new AdminSportFormViewModel
        {
            Id = source.Id,
            Name = source.Name,
            Icon = source.Icon,
            DefaultMinPlayers = source.DefaultMinPlayers,
            DefaultMaxPlayers = source.DefaultMaxPlayers
        };

        return result;
    }

    public AdminSportCreateRequestData
        SportFormToCreateRequestData(AdminSportFormViewModel source)
    {
        var result = new AdminSportCreateRequestData
        {
            Name = source.Name,
            Icon = source.Icon,
            DefaultMinPlayers = source.DefaultMinPlayers,
            DefaultMaxPlayers = source.DefaultMaxPlayers
        };

        return result;
    }

    public AdminSportUpdateRequestData
        SportFormToUpdateRequestData(Guid sportId, AdminSportFormViewModel source)
    {
        var result = new AdminSportUpdateRequestData
        {
            SportId = sportId,
            Name = source.Name,
            Icon = source.Icon,
            DefaultMinPlayers = source.DefaultMinPlayers,
            DefaultMaxPlayers = source.DefaultMaxPlayers
        };

        return result;
    }
}
