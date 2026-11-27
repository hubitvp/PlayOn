
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using PlayOn.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace PlayOn.Web.Validation
{
    public sealed class DurationStepAttributeAdapterProvider
        : IValidationAttributeAdapterProvider
    {
        private readonly IValidationAttributeAdapterProvider defaultProvider =
            new ValidationAttributeAdapterProvider();

        public IAttributeAdapter? GetAttributeAdapter(
            ValidationAttribute attribute,
            IStringLocalizer? stringLocalizer)
        {
            if (attribute is DurationStepAttribute durationStepAttribute)
            {
                return new DurationStepAttributeAdapter(
                    durationStepAttribute,
                    stringLocalizer);
            }

            return defaultProvider.GetAttributeAdapter(
                attribute,
                stringLocalizer);
        }
    }
}
