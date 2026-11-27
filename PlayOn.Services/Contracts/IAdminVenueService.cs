using PlayOn.Common.Helpers;
using PlayOn.Services.Data;
using PlayOn.Services.Data.AdminVenues.RequestData;
using PlayOn.Services.Data.AdminVenues.ResponseModels;

namespace PlayOn.Services.Contracts
{
    /// <summary>
    /// Defines administrative management operations for venue slots.
    /// </summary>
    public interface IAdminVenueService
    {
        /// <summary>
        /// Gets a filtered, paged list of venue slots.
        /// </summary>
        /// <param name="request">The search, status filter and paging options.</param>
        /// <returns>A page of venues, or a failure response when the request is invalid.</returns>
        Task<ServiceResponse<PageResult<AdminVenueResponseModel>>>
            GetVenuesAsync(AdminVenueQueryRequestData request);

        /// <summary>
        /// Gets the data needed to edit a venue slot.
        /// </summary>
        /// <param name="venueId">The identifier of the venue slot.</param>
        /// <returns>The venue edit model, or a not-found response when it does not exist.</returns>
        Task<ServiceResponse<AdminVenueEditResponseModel>>
            GetForEditAsync(Guid venueId);

        /// <summary>
        /// Creates a new venue slot.
        /// </summary>
        /// <param name="request">The data of the venue slot to create.</param>
        /// <returns>The identifier of the created venue, or a failure response when validation fails.</returns>
        Task<ServiceResponse<Guid>>
            CreateAsync(AdminVenueCreateRequestData request);

        /// <summary>
        /// Updates an existing venue slot.
        /// </summary>
        /// <param name="request">The updated venue data, including the venue identifier.</param>
        /// <returns>The identifier of the updated venue, or a not-found, conflict or failure response.</returns>
        Task<ServiceResponse<Guid>>
            UpdateAsync(AdminVenueUpdateRequestData request);

        /// <summary>
        /// Soft-deletes a venue slot.
        /// </summary>
        /// <param name="venueId">The identifier of the venue slot to delete.</param>
        /// <returns>The identifier of the venue, or a not-found or conflict response.</returns>
        Task<ServiceResponse<Guid>>
            DeleteAsync(Guid venueId);

        /// <summary>
        /// Restores a previously deleted venue slot.
        /// </summary>
        /// <param name="venueId">The identifier of the venue slot to restore.</param>
        /// <returns>The identifier of the venue, or a not-found response.</returns>
        Task<ServiceResponse<Guid>>
            RestoreAsync(Guid venueId);
    }
}
