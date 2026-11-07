using System.ComponentModel.DataAnnotations;

namespace PlayOn.Web.ViewModels.Home
{
    public class HomeSearchInputModel : IValidatableObject
    {
        public int? SportId { get; set; }

        public string? City { get; set; }

        public string? From { get; set; }

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            var timeProvider = (TimeProvider?)validationContext
                .GetService(typeof(TimeProvider))
                ?? throw new InvalidOperationException(
                    "TimeProvider is not registered.");

            if (SportId.HasValue && SportId.Value <= 0)
            {
                yield return new ValidationResult(
                    "Sport ID must be a positive integer.",
                    new[] { nameof(SportId) });
            }

            if (!string.IsNullOrWhiteSpace(City) && City.Length > 100)
            {
                yield return new ValidationResult(
                    "City name cannot exceed 100 characters.",
                    new[] { nameof(City) });
            }

            if (!string.IsNullOrWhiteSpace(From))
            {
                if (!DateOnly.TryParse(From, out var parsedFrom))
                {
                    yield return new ValidationResult(
                        "Invalid date format.",
                        new[] { nameof(From) });
                }
                else if (parsedFrom < DateOnly.FromDateTime(
                    timeProvider.GetLocalNow().DateTime))
                {
                    yield return new ValidationResult(
                        "The date cannot be in the past.",
                        new[] { nameof(From) });
                }
            }
        }
    }
}
