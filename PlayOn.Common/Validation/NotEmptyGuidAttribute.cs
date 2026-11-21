using System.ComponentModel.DataAnnotations;

namespace PlayOn.Common.Validation
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed class NotEmptyGuidAttribute : ValidationAttribute
    {
        public NotEmptyGuidAttribute()
            : base("The value is not valid.")
        {
        }

        public override bool IsValid(object? value)
        {
            return value is null || (value is Guid guid && guid != Guid.Empty);
        }
    }
}
