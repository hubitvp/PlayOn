using PlayOn.Services.Data;
using PlayOn.Services.Data.Home.ResponseModels;

namespace PlayOn.Services.Contracts
{
    public interface ISportService
    {
        Task<ServiceResponse<IEnumerable<SportsResponseModel>>>
           GetSportsAsync();
    }
}
