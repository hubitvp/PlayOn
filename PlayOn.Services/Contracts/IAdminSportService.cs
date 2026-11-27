using PlayOn.Common.Helpers;
using PlayOn.Services.Data;
using PlayOn.Services.Data.AdminSports.RequestData;
using PlayOn.Services.Data.AdminSports.ResponseModels;

namespace PlayOn.Services.Contracts
{
    /// <summary>
    /// Defines administrative management operations for sports.
    /// </summary>
    public interface IAdminSportService
    {
        /// <summary>
        /// Gets a filtered, paged list of sports.
        /// </summary>
        /// <param name="request">The search and paging options.</param>
        /// <returns>A page of sports, or a failure response when the request is invalid.</returns>
        Task<ServiceResponse<PageResult<AdminSportResponseModel>>>
            GetSportsAsync(AdminSportQueryRequestData request);

        /// <summary>
        /// Gets the data needed to edit a sport.
        /// </summary>
        /// <param name="sportId">The identifier of the sport.</param>
        /// <returns>The sport edit model, or a not-found response when the sport does not exist.</returns>
        Task<ServiceResponse<AdminSportEditResponseModel>>
            GetForEditAsync(Guid sportId);

        /// <summary>
        /// Creates a new sport.
        /// </summary>
        /// <param name="request">The data of the sport to create.</param>
        /// <returns>The identifier of the created sport, or a failure response when validation fails or the name is taken.</returns>
        Task<ServiceResponse<Guid>>
            CreateAsync(AdminSportCreateRequestData request);

        /// <summary>
        /// Updates an existing sport.
        /// </summary>
        /// <param name="request">The updated sport data, including the sport identifier.</param>
        /// <returns>The identifier of the updated sport, or a not-found or failure response.</returns>
        Task<ServiceResponse<Guid>>
            UpdateAsync(AdminSportUpdateRequestData request);

        /// <summary>
        /// Deletes a sport that is not in use.
        /// </summary>
        /// <param name="sportId">The identifier of the sport to delete.</param>
        /// <returns>The identifier of the deleted sport, or a not-found or conflict response.</returns>
        Task<ServiceResponse<Guid>>
            DeleteAsync(Guid sportId);
    }
}
