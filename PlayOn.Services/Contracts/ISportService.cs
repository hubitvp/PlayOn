using PlayOn.Services.Data;
using PlayOn.Services.Data.Sport.ResponseModels;

namespace PlayOn.Services.Contracts
{
    public interface ISportService
    {
        Task<ServiceResponse<IReadOnlyList<SportResponseModel>>>
            GetSportOptionsAsync();
    }
}
