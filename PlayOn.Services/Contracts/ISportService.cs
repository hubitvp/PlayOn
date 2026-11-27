using PlayOn.Services.Data;
using PlayOn.Services.Data.Sport.ResponseModels;

namespace PlayOn.Services.Contracts
{
    /// <summary>
    /// Defines the operations for retrieving sports.
    /// </summary>
    public interface ISportService
    {
        /// <summary>
        /// Retrieves all available sports.
        /// </summary>
        /// <returns>A response with the sports.</returns>
        Task<ServiceResponse<IReadOnlyList<SportResponseModel>>>
            GetSportOptionsAsync();
    }
}
