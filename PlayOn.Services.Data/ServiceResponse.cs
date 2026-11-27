using System.ComponentModel.DataAnnotations;

namespace PlayOn.Services.Data
{
    /// <summary>
    /// Describes the outcome of a service operation.
    /// </summary>
    public enum ServiceStatus
    {
        /// <summary>
        /// The operation completed successfully.
        /// </summary>
        Success,

        /// <summary>
        /// The input was invalid; see the validation errors.
        /// </summary>
        InvalidInput,

        /// <summary>
        /// The requested resource was not found.
        /// </summary>
        NotFound,

        /// <summary>
        /// The caller is not allowed to perform the operation.
        /// </summary>
        Forbidden,

        /// <summary>
        /// The operation is blocked because of a time restriction.
        /// </summary>
        TimeLocked,

        /// <summary>
        /// The operation conflicts with the current state of the data, for example a concurrent modification.
        /// </summary>
        Conflict
    }

    /// <summary>
    /// Represents the result of a service operation: its outcome, any validation errors and the returned data.
    /// </summary>
    /// <typeparam name="TData">The type of the data returned by the operation.</typeparam>
    public class ServiceResponse<TData>
    {
        /// <summary>
        /// Indicates whether the operation completed successfully.
        /// </summary>
        public bool IsSuccess { get; init; }
        /// <summary>
        /// The outcome of the operation.
        /// </summary>
        public ServiceStatus Status { get; init; }
        /// <summary>
        /// Validation errors describing why the operation failed; empty when there are none.
        /// </summary>
        public IEnumerable<ValidationResult> Errors { get; init; } = Enumerable.Empty<ValidationResult>();

        /// <summary>
        /// The data returned by the operation; the default value when the operation failed.
        /// </summary>
        public TData ResponseData { get; init; } = default!;

        /// <summary>
        /// Creates a successful response carrying the given data.
        /// </summary>
        /// <param name="responseData">The data returned by the operation.</param>
        /// <returns>A successful <see cref="ServiceResponse{TData}"/>.</returns>
        public static ServiceResponse<TData> Success(TData responseData)
        {
            return new ServiceResponse<TData>
            {
                IsSuccess = true,
                Status = ServiceStatus.Success,
                ResponseData = responseData
            };
        }

        /// <summary>
        /// Creates a failed response with <see cref="ServiceStatus.InvalidInput"/> and the given validation errors.
        /// </summary>
        /// <param name="errors">The validation errors describing what was wrong with the input.</param>
        /// <returns>A failed <see cref="ServiceResponse{TData}"/>.</returns>
        public static ServiceResponse<TData> Failure(
            IEnumerable<ValidationResult> errors)
        {
            return new ServiceResponse<TData>
            {
                IsSuccess = false,
                Status = ServiceStatus.InvalidInput,
                Errors = errors,
            };
        }


        /// <summary>
        /// Creates a failed response with <see cref="ServiceStatus.InvalidInput"/> and a single error message.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="memberName">The name of the member the error relates to; empty for a general error.</param>
        /// <returns>A failed <see cref="ServiceResponse{TData}"/>.</returns>
        public static ServiceResponse<TData>
            Failure(string message, string memberName = "")
        {
            return Failure(new[] { new ValidationResult(message, new[] { memberName }) });
        }

        /// <summary>
        /// Creates a response with <see cref="ServiceStatus.NotFound"/>, indicating the requested resource does not exist.
        /// </summary>
        /// <returns>A <see cref="ServiceResponse{TData}"/> with the not found status.</returns>
        public static ServiceResponse<TData> NotFound()
        {
            return new ServiceResponse<TData> { Status = ServiceStatus.NotFound };
        }

        /// <summary>
        /// Creates a response with <see cref="ServiceStatus.Forbidden"/>, indicating the caller is not allowed to perform the operation.
        /// </summary>
        /// <returns>A <see cref="ServiceResponse{TData}"/> with the forbidden status.</returns>
        public static ServiceResponse<TData> Forbidden()
        {
            return new ServiceResponse<TData> { Status = ServiceStatus.Forbidden };
        }


        /// <summary>
        /// Creates a response with <see cref="ServiceStatus.TimeLocked"/>, indicating the operation is blocked because of a time restriction, and carrying data for the caller to show.
        /// </summary>
        /// <param name="responseData">The data to return together with the status.</param>
        /// <returns>A <see cref="ServiceResponse{TData}"/> with the time locked status.</returns>
        public static ServiceResponse<TData> TimeLocked(TData responseData)
        {
            return new ServiceResponse<TData>
            {
                Status = ServiceStatus.TimeLocked,
                ResponseData = responseData,
            };
        }


        /// <summary>
        /// Creates a response with <see cref="ServiceStatus.Conflict"/> and an error message describing the conflict.
        /// </summary>
        /// <param name="message">The error message describing the conflict.</param>
        /// <returns>A <see cref="ServiceResponse{TData}"/> with the conflict status.</returns>
        public static ServiceResponse<TData> Conflict(string message)
        {
            return new ServiceResponse<TData>
            {
                Status = ServiceStatus.Conflict,
                Errors = new[] { new ValidationResult(message) }
            };
        }

        /// <summary>
        /// Creates a response with <see cref="ServiceStatus.Conflict"/> and no error message.
        /// </summary>
        /// <returns>A <see cref="ServiceResponse{TData}"/> with the conflict status.</returns>
        public static ServiceResponse<TData> Conflict()
        {
            return new ServiceResponse<TData>
            {
                Status = ServiceStatus.Conflict,
            };
        }
    }
}
