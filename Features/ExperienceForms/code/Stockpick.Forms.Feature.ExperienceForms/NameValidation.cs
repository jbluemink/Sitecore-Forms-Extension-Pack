using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

using System.Text.RegularExpressions;

using System.Web.Mvc;

using Sitecore.ExperienceForms.Mvc.Models.Fields;

using Sitecore.ExperienceForms.Mvc.Models.Validation;

namespace Sitecore.ExperienceForms.Samples.Validation

{

    public class NameValidation : ValidationElement<string>

    {

        private const string NamePattern = "^[a-zA-Z ]*$";

        public NameValidation(ValidationDataModel validationItem) : base(validationItem)

        {

        }

        public override IEnumerable<ModelClientValidationRule> ClientValidationRules

        {

            get

            {

                var clientValidationRule = new ModelClientValidationRule

                {

                    ErrorMessage = FormatMessage(Title),

                    ValidationType = "regex"

                };

                clientValidationRule.ValidationParameters.Add("pattern", NamePattern);

                yield return clientValidationRule;

            }

        }

        public string Title { get; set; }

        public override ValidationResult Validate(object value)

        {

            if (value == null)

            {

                return ValidationResult.Success;

            }

            var regex = new Regex(NamePattern, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled);

            var stringValue = (string)value;

            if (string.IsNullOrEmpty(stringValue) || regex.IsMatch(stringValue))

            {

                return ValidationResult.Success;

            }

            return new ValidationResult(FormatMessage(Title));

        }

        public override void Initialize(object validationModel)

        {

            base.Initialize(validationModel);

            var obj = validationModel as StringInputViewModel;

            if (obj != null)

            {

                Title = obj.Title;

            }

        }

    }

}