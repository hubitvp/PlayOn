using PlayOn.Services.Data;
using PlayOn.Services.Data.Attendance.RequestData;
using PlayOn.Services.Data.Attendance.ResponseModels;

namespace PlayOn.Services.Contracts
{
    /// <summary>
    /// Records and retrieves attendance for played matches.
    /// </summary>
    public interface IAttendanceService
    {
        /// <summary>
        /// Gets the attendance sheet (match info and joined players) for a match.
        /// </summary>
        /// <param name="matchId">The identifier of the match.</param>
        /// <param name="actorId">The identifier of the user making the request.</param>
        /// <param name="actorIsAdmin">Whether the requesting user is an administrator.</param>
        /// <returns>A response with the attendance sheet, or a not-found, forbidden or conflict result when the match is missing, not accessible, cancelled, already recorded or not yet ended.</returns>
        Task<ServiceResponse<AttendanceResponseModel>>
            GetAsync(
                Guid matchId,
                Guid actorId,
                bool actorIsAdmin);

        /// <summary>
        /// Submits attendance for every joined player of a match, completes the match and updates player statistics.
        /// </summary>
        /// <param name="request">The attendance submission data.</param>
        /// <returns>A response with the match identifier on success, or a validation failure, not-found, forbidden or conflict result.</returns>
        Task<ServiceResponse<Guid>>
            SubmitAsync(AttendanceRequestData request);
    }
}
