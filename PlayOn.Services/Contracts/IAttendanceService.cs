using PlayOn.Services.Data;
using PlayOn.Services.Data.Attendance.RequestData;
using PlayOn.Services.Data.Attendance.ResponseModels;

namespace PlayOn.Services.Contracts
{
    public interface IAttendanceService
    {
        Task<ServiceResponse<AttendanceResponseModel>>
            GetAsync(
                Guid matchId,
                Guid actorId,
                bool actorIsAdmin);

        Task<ServiceResponse<Guid>>
            SubmitAsync(AttendanceRequestData request);
    }
}
