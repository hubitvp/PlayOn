using System.ComponentModel.DataAnnotations;

namespace PlayOn.Services.Data
{
    public enum ServiceStatus
    {
        Success,

        Invalid,
        NotFound,

        Forbidden,

        Conflict
    }

    public class ServiceResponse<TData>
    {
        public bool IsSuccess { get; init; }
        public ServiceStatus Status { get; init; }
        public IEnumerable<ValidationResult> Errors { get; init; } = Enumerable.Empty<ValidationResult>();

        public TData ResponseData { get; init; } = default!;

        public static ServiceResponse<TData> Success(TData responseData)
        {
            return new ServiceResponse<TData>
            {
                IsSuccess = true,
                Status = ServiceStatus.Success,
                ResponseData = responseData
            };
        }

        public static ServiceResponse<TData> Failure(
            IEnumerable<ValidationResult> errors)
        {
            return new ServiceResponse<TData>
            {
                IsSuccess = false,
                Status = ServiceStatus.Invalid,
                Errors = errors,
            };
        }


        public static ServiceResponse<TData>
            Failure(string message, string memberName = "")
        {
            return Failure(new[] { new ValidationResult(message, new[] { memberName }) });
        }

        public static ServiceResponse<TData> NotFound()
        {
            return new ServiceResponse<TData> { Status = ServiceStatus.NotFound };
        }

        public static ServiceResponse<TData> Forbidden()
        {
            return new ServiceResponse<TData> { Status = ServiceStatus.Forbidden };
        }

        public static ServiceResponse<TData> Conflict(string message)
        {
            return new ServiceResponse<TData>
            {
                Status = ServiceStatus.Conflict,
                Errors = new[] { new ValidationResult(message) }
            };
        }
    }
}
