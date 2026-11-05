using System.ComponentModel.DataAnnotations;

namespace PlayOn.Services.Data
{
    public static class ServiceRequestDataValidator
    {
        public static IEnumerable<ValidationResult> Validate(object instance)
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
