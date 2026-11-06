using System.ComponentModel.DataAnnotations;

namespace PlayOn.Services.Data
{
    public class ServiceResponse<TData>
    {
        public bool IsSuccess { get; init; }
        public IEnumerable<ValidationResult> Errors { get; init; } = Enumerable.Empty<ValidationResult>();

        public TData ResponseData { get; init; } = default!;

        public static ServiceResponse<TData> Success(TData responseData)
        {
            return new ServiceResponse<TData>
            {
                IsSuccess = true,
                ResponseData = responseData
            };
        }

        public static ServiceResponse<TData> Failure(
            IEnumerable<ValidationResult> errors)
        {
            return new ServiceResponse<TData>
            {
                IsSuccess = false,
                Errors = errors
            };
        }
    }
}
