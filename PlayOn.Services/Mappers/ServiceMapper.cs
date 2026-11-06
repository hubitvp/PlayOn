using Riok.Mapperly.Abstractions;
using PlayOn.Services.Data.Home.RequestData;
using PlayOn.Services.Data.Home.ResponseModels;
using PlayOn.Services.Data.Venue.ResponseModels;
using PlayOn.Services.Data.Venue.RequestData;
using PlayOn.Services.Repositories.Data.Input;
using PlayOn.Services.Repositories.Data.Output;

namespace PlayOn.Services.Mappers;

[Mapper]
public partial class ServiceMapper
{
    public partial MatchQueryDto ToMatchQuery(HomeQueryRequestData source);

    public partial VenueQueryDto ToVenueQuery(VenueQueryRequestData source);

    public partial MatchCardResponseModel ToMatchCardResponse(MatchCardDto source);

    public partial SportsResponseModel ToSportsResponse(SportDto source);

    public partial VenueResponseModel ToVenueResponse(VenueDto source);

    public partial CourtResponseModel ToCourtResponse(CourtDto source);
}
