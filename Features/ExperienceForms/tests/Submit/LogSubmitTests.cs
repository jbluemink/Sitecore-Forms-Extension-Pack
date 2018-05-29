using NSubstitute;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.ExperienceForms.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Stockpick.Forms.Feature.ExperienceForms.Submit;

namespace Stockpick.Forms.Feature.ExperienceForms.Tests.Submit
{
    [TestFixture]
    public class LogSubmitTests : LogSubmit
    {
        protected override ILogger Logger { get; }

        public LogSubmitTests(ISubmitActionData submitActionData) : base(submitActionData)
        {
        }

        public LogSubmitTests(ILogger logger) : base(Substitute.For<ISubmitActionData>())
        {
            this.Logger = logger;
        }

        public LogSubmitTests() : base(Substitute.For<ISubmitActionData>())
        {
        }

        [Test]
        public void Execute_FormSubmitContextHasErrors_LogsWarning()
        {
            // Arrange
            var logger = Substitute.For<ILogger>();
            var logSubmit = new LogSubmitTests(logger);
            var formSubmitContext = Substitute.ForPartsOf<FormSubmitContext>(ID.NewID.ToString());
            formSubmitContext.Errors.Add(Substitute.For<FormActionError>());

            // Act
            var ret = logSubmit.Execute("data", formSubmitContext);

            // Assert
            logger.ReceivedWithAnyArgs().Warn(Arg.Any<string>(), Arg.Any<LogSubmit>());
        }

        [Test]
        public void Execute_FormSubmitContextHasNoErrors_LogsInfo()
        {
            // Arrange
            var logger = Substitute.For<ILogger>();
            var logSubmit = new LogSubmitTests(logger);
            var formSubmitContext = Substitute.ForPartsOf<FormSubmitContext>(ID.NewID.ToString());

            // Act
            var ret = logSubmit.Execute("data", formSubmitContext);

            // Assert
            logger.ReceivedWithAnyArgs().Info(Arg.Any<string>(), Arg.Any<LogSubmit>());
        }

        [Test]
        public void Execute_AnyCase_ReturnsTrue()
        {
            // Arrange
            var logger = Substitute.For<ILogger>();
            var logSubmit = new LogSubmitTests(logger);
            var formSubmitContext = Substitute.ForPartsOf<FormSubmitContext>(ID.NewID.ToString());

            // Act
            var ret = logSubmit.Execute("data", formSubmitContext);

            // Assert
            Assert.That(ret, Is.True);
        }
    }
}