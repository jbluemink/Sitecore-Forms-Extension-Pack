using NSubstitute;
using NUnit.Framework;
using Sitecore.ExperienceForms.Mvc.Models.Fields;
using Sitecore.ExperienceForms.Mvc.Models.Validation;
using Stockpick.Forms.Feature.ExperienceForms.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stockpick.Forms.Feature.ExperienceForms.Tests.Validation
{
    [TestFixture]
    public class NameValidationTests
    {

        [Test]
        public void ClientValidationRules_IsNotEmpty()
        {
            // Arrange
            var nameValidation = new NameValidation(Substitute.ForPartsOf<ValidationDataModel>());

            // Act

            // Assert
            Assert.That(nameValidation.ClientValidationRules, Is.Not.Empty);

        }

        [Test]
        public void ClientValidationRules_ContainsRegexValidationType()
        {
            // Arrange
            var nameValidation = new NameValidation(Substitute.ForPartsOf<ValidationDataModel>());

            // Act

            // Assert
            Assert.That(nameValidation.ClientValidationRules, Has.One.Property("ValidationType").EqualTo("regex"));

        }

        [Test]
        public void Validate_ValidExpression_ReturnValidationResultSuccess()
        {
            // Arrange
            var nameValidation = new NameValidation(Substitute.ForPartsOf<ValidationDataModel>());
            var expression = "arara";

            // Act
            var ret = nameValidation.Validate(expression);

            // Assert
            /*
             * ValidationResult.Success is always null if succeed
             */
            Assert.That(ret, Is.Null);

        }

        [Test]
        public void Validate_NullExpression_ReturnValidationResultSuccess()
        {
            // Arrange
            var nameValidation = new NameValidation(Substitute.ForPartsOf<ValidationDataModel>());
            var expression = "arara";

            // Act
            var ret = nameValidation.Validate(null);

            // Assert
            /*
             * ValidationResult.Success is always null if succeed
             */
            Assert.That(ret, Is.Null);

        }

        [Test]
        public void Validate_InvalidExpression_ReturnValidationResultFail()
        {
            // Arrange
            var nameValidation = new NameValidation(Substitute.ForPartsOf<ValidationDataModel>());
            var expression = "ara!@#!ra";

            // Act
            var ret = nameValidation.Validate(expression);

            // Assert
            /*
             * ValidationResult.Success is always null if succeed
             */
            Assert.That(ret, Is.Not.Null);

        }

        [Test]
        public void Initialize_ValidationModelIsStringInputViewModel_SetTitle()
        {
            // Arrange
            var nameValidation = new NameValidation(Substitute.ForPartsOf<ValidationDataModel>());

            // Act
            var title = "foo";
            var validationModel = new StringInputViewModel { Title = title };
            nameValidation.Initialize(validationModel);

            // Assert
            Assert.That(nameValidation.Title, Is.EqualTo(title));

        }

        [Test]
        public void Initialize_ValidationModelIsNotStringInputViewModel_TitleIsNull()
        {
            // Arrange
            var nameValidation = new NameValidation(Substitute.ForPartsOf<ValidationDataModel>());

            // Act
            var validationModel = new object { };
            nameValidation.Initialize(validationModel);

            // Assert
            Assert.That(nameValidation.Title, Is.Null);

        }

    }
}
