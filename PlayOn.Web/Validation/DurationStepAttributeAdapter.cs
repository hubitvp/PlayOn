
using System.Globalization;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using PlayOn.Common.Validation;

namespace PlayOn.Validation
{
    public sealed class DurationStepAttributeAdapter
        : AttributeAdapterBase<DurationStepAttribute>
    {
        public DurationStepAttributeAdapter(
            DurationStepAttribute attribute,
            IStringLocalizer? stringLocalizer)
            : base(attribute, stringLocalizer)
        {
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");

            MergeAttribute(
                context.Attributes,
                "data-val-durationstep",
                GetErrorMessage(context));

            MergeAttribute(
                context.Attributes,
                "data-val-durationstep-step",
                Attribute.Step.ToString(CultureInfo.InvariantCulture));
        }

        public override string GetErrorMessage(
            ModelValidationContextBase validationContext)
        {
            return Attribute.FormatErrorMessage(
                validationContext.ModelMetadata.GetDisplayName());
        }

        private static bool MergeAttribute(
            IDictionary<string, string> attributes,
            string key,
            string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }

            attributes.Add(key, value);
            return true;
        }
    }
}
