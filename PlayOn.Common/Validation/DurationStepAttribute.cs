
using PlayOn.Common.ApplicationConstants;
using System.ComponentModel.DataAnnotations;

namespace PlayOn.Common.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class DurationStepAttribute : ValidationAttribute
    {
        public int Step { get; }

        public DurationStepAttribute()
        {
            Step = MatchRules.DurationStepInMinutes;
            ErrorMessage = $"Duration must be a multiple of {Step} minutes.";
        }

        public override bool IsValid(object? value)
        {
            if (value is null)
            {
                return true;
            }

            return value is int duration && duration % Step == 0;
        }
    }
}
