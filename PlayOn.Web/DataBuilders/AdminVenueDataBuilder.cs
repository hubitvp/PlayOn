using PlayOn.Common.Helpers;
using PlayOn.Services.Data.AdminVenues.RequestData;
using PlayOn.Services.Data.AdminVenues.ResponseModels;
using PlayOn.Web.DataBuilders.Formatting;
using PlayOn.Web.ViewModels.Admin;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Web.DataBuilders;

public class AdminVenueDataBuilder
{
    private readonly IDisplayFormatter displayFormatter;
    private readonly TimeProvider timeProvider;

    public AdminVenueDataBuilder(
        IDisplayFormatter displayFormatter,
        TimeProvider timeProvider)
    {
        this.displayFormatter = displayFormatter;
        this.timeProvider = timeProvider;
    }

    public AdminVenueQueryRequestData
        VenueQueryToRequestData(AdminVenueSearchInputModel input)
    {
        var result = new AdminVenueQueryRequestData
        {
            Search = input.Search,
            Status = input.Status,
            Page = input.Page,
            PageSize = AdminPageSize
        };

        return result;
    }

    public AdminVenueListViewModel
        VenueListToViewModel(
            PageResult<AdminVenueResponseModel>? page,
            AdminVenueSearchInputModel input)
    {
        var result = new AdminVenueListViewModel
        {
            Venues = page?.Items
                .Select(VenueToViewModel)
                .ToList() ?? [],
            Search = input.Search,
            Status = input.Status,
            TotalCount = page?.TotalCount ?? 0,
            Page = page?.Page ?? input.Page,
            PageSize = page?.PageSize ?? AdminPageSize,
            TotalPages = page?.TotalPages ?? 0
        };

        return result;
    }

    public AdminVenueItemViewModel
        VenueToViewModel(AdminVenueResponseModel source)
    {
        var result = new AdminVenueItemViewModel
        {
            Id = source.Id,
            Name = source.Name,
            Address = source.Address,
            City = source.City,
            Date = displayFormatter.ShortDate(source.Date),
            StartsAt = displayFormatter.Time(source.StartsAt),
            OpeningHours = $"{displayFormatter.Time(source.OpensAt)} - {displayFormatter.Time(source.ClosesAt)}",
            IsDeleted = source.IsDeleted,
            IsBooked = source.IsBooked,
            MatchTitle = source.MatchTitle
        };

        return result;
    }

    public AdminVenueFormViewModel
        NewVenueForm()
    {
        var tomorrow = DateOnly.FromDateTime(timeProvider.GetLocalNow().DateTime).AddDays(1);

        var result = new AdminVenueFormViewModel
        {
            Date = tomorrow,
            OpensAt = new TimeOnly(8, 0),
            ClosesAt = new TimeOnly(22, 0),
            StartsAt = new TimeOnly(18, 0)
        };

        return result;
    }

    public AdminVenueFormViewModel
        VenueToFormViewModel(AdminVenueEditResponseModel source)
    {
        var result = new AdminVenueFormViewModel
        {
            Id = source.Id,
            IsBooked = source.IsBooked,
            Name = source.Name,
            Address = source.Address,
            City = source.City,
            Latitude = source.Latitude,
            Longitude = source.Longitude,
            Date = source.Date,
            StartsAt = source.StartsAt,
            OpensAt = source.OpensAt,
            ClosesAt = source.ClosesAt,
            PhotoUrl = source.PhotoUrl
        };

        return result;
    }

    public AdminVenueCreateRequestData
        VenueFormToCreateRequestData(AdminVenueFormViewModel source)
    {
        var result = new AdminVenueCreateRequestData();

        CopyForm(source, result);

        return result;
    }

    public AdminVenueUpdateRequestData
        VenueFormToUpdateRequestData(Guid venueId, AdminVenueFormViewModel source)
    {
        var result = new AdminVenueUpdateRequestData
        {
            VenueId = venueId
        };

        CopyForm(source, result);

        return result;
    }

    private static void CopyForm(AdminVenueFormViewModel source, AdminVenueRequestDataBase target)
    {
        target.Name = source.Name;
        target.Address = source.Address;
        target.City = source.City;
        target.Latitude = source.Latitude;
        target.Longitude = source.Longitude;
        target.Date = source.Date;
        target.StartsAt = source.StartsAt;
        target.OpensAt = source.OpensAt;
        target.ClosesAt = source.ClosesAt;
        target.PhotoUrl = source.PhotoUrl;
    }
}
