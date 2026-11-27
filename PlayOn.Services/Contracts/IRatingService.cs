using PlayOn.Services.Data;
using PlayOn.Services.Data.Ratings.RequestData;
using PlayOn.Services.Data.Ratings.ResponseModels;

namespace PlayOn.Services.Contracts
{
    /// <summary>
    /// Defines operations for rating fellow attendees of a completed match.
    /// </summary>
    public interface IRatingService
    {
        /// <summary>
        /// Gets the rating form listing the attendees the rater has not rated yet.
        /// </summary>
        /// <param name="matchId">The identifier of the match.</param>
        /// <param name="raterId">The identifier of the rating user.</param>
        /// <returns>A service response with the form, or a failure if the rater may not rate.</returns>
        Task<ServiceResponse<RatingFormResponseModel>>
            GetFormAsync(
                Guid matchId,
                Guid raterId);

        /// <summary>
        /// Validates and saves the submitted ratings and notifies the rated players.
        /// </summary>
        /// <param name="request">The rating request with the match, rater and entries.</param>
        /// <returns>A service response with the number of ratings saved.</returns>
        Task<ServiceResponse<int>>
            SubmitAsync(RatingRequestData request);
    }
}
