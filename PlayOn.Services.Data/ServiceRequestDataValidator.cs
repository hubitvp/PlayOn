using System.ComponentModel.DataAnnotations;

namespace PlayOn.Services.Data
{
    /// <summary>
    /// Helper for validating service request data objects with data annotations.
    /// </summary>
    public static class ServiceRequestDataValidator
    {
        /// <summary>
        /// Validates the given object against its data annotation attributes and, if those pass, its <see cref="IValidatableObject"/> implementation.
        /// </summary>
        /// <param name="instance">The object to validate.</param>
        /// <returns>A collection of validation errors; empty if the object is valid.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="instance"/> is <c>null</c>.</exception>
        public static ICollection<ValidationResult> Validate(object instance)
        {
            ArgumentNullException.ThrowIfNull(instance);

            var errors = new List<ValidationResult>();

            var context = new ValidationContext(instance);

            Validator.TryValidateObject(
                instance,
                context,
                errors,
                validateAllProperties: true);

            return errors;
        }
    }
}
