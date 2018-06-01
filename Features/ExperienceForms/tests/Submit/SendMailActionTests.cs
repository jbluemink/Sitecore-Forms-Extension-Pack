using NSubstitute;
using NUnit.Framework;
using Sitecore;
using Sitecore.Data;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Mvc.Models.Fields;
using Sitecore.ExperienceForms.Processing;
using Sitecore.FakeDb;
using Sitecore.FakeDb.Sites;
using Stockpick.Forms.Feature.ExperienceForms.Model.SendMail;
using Stockpick.Forms.Feature.ExperienceForms.Submit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Stockpick.Forms.Feature.ExperienceForms.Tests.Submit
{
    [TestFixture]
    public class SendMailActionTests : SendMailAction
    {

        private const string EmailTemplateSample = "{F0488100-24E0-4A9D-847D-D732AF8BB2A1}";

        private FakeSiteContext BuildSite()
        {
            return new Sitecore.FakeDb.Sites.FakeSiteContext(
                new Sitecore.Collections.StringDictionary { { "name", "website" }, { "database", "web" } });
        }

        private DbItem BuildSettings()
        {

            var testEmailMessage = new DbItem("Test E-mail Message", new ID(EmailTemplateSample), EmailTemplate.TemplateID);
            testEmailMessage.Fields.Add(new DbField("Subject", EmailTemplate.FieldIds.Subject) { Value = "Hardcoded part | {Subject}" });
            testEmailMessage.Fields.Add(new DbField("From", EmailTemplate.FieldIds.From) { Value = "{From}" });
            testEmailMessage.Fields.Add(new DbField("To", EmailTemplate.FieldIds.To) { Value = "hardcoded@test.com, {To}" });
            testEmailMessage.Fields.Add(new DbField("Cc", EmailTemplate.FieldIds.Cc) { Value = "hardcoded@test.com, {CC}" });
            testEmailMessage.Fields.Add(new DbField("Bcc", EmailTemplate.FieldIds.Bcc) { Value = "{BCC}" });
            testEmailMessage.Fields.Add(new DbField("Message RichText", EmailTemplate.FieldIds.MessageRichText) { Value = "<p>And here goes the Richtext message</p><p><strong>Subject:</strong> {Subject}</p><p><strong>From:</strong>&nbsp;{From}</p><p><strong>To:</strong>&nbsp;{To}</p><p><strong>CC:</strong>&nbsp;{CC}</p><p><strong>BCC:</strong>&nbsp;{BCC}</p><p><strong>Message:</strong>&nbsp;{Message}</p><p><strong>Number:</strong>&nbsp;{Number}</p><p><strong>Phone:</strong>&nbsp;{Phone}</p><p><strong>Checkbox:</strong>&nbsp;{Checkbox}</p><p><strong>Date:</strong>&nbsp;{Date}</p><p><strong>Dropdown:</strong>&nbsp;{Dropdown}</p><p><strong>Listbox:</strong>&nbsp;{Listbox}</p><p><strong>CheckboxList:</strong>&nbsp;{CheckboxList}</p><p><strong>RadioButtonList:</strong>&nbsp;{RadioButtonList}</p><p><strong>Password:</strong>&nbsp;{Password}</p><p>&nbsp;</p>" });
            testEmailMessage.Fields.Add(new DbField("Message Text", EmailTemplate.FieldIds.MessageText) { Value = "Hello World" });

            var settings = new DbItem("Settings")
            {
                 new DbItem("Forms")
                 {
                    new DbItem("Email Templates")
                    {
                        testEmailMessage
                    }
                 }
            };

            settings.ParentID = ItemIDs.SystemRoot;
            return settings;

        }

        private DbItem BuildDictionaries()
        {

            var dictionary = new DbItem("Dictionary", ItemIDs.Dictionary, TemplateIDs.DictionaryDomain)
            {
                new DbItem("Forms")
                {
                     new DbItem("Actions")
                     {
                        new DbItem("SendMail")
                        {
                            new DbItem("Checkbox Checked Text", ID.NewID, TemplateIDs.DictionaryEntry)
                            {
                                {"Key","Forms.Actions.SendMail.CheckboxCheckedText" },
                                {"Phrase","Checked" }
                            },
                            new DbItem("Checkbox Unchecked Text", ID.NewID, TemplateIDs.DictionaryEntry)
                            {
                                {"Key","Forms.Actions.SendMail.CheckboxUncheckedText" },
                                {"Phrase","Unchecked" }
                            },
                            new DbItem("Date Format Mask", ID.NewID, TemplateIDs.DictionaryEntry)
                            {
                                {"Key","Forms.Actions.SendMail.DateFormatMask" },
                                {"Phrase","MM/dd/yyyy" }
                            },
                            new DbItem("Double Format Mask", ID.NewID, TemplateIDs.DictionaryEntry)
                            {
                                {"Key","Forms.Actions.SendMail.DoubleFormatMask" },
                                {"Phrase","F0" }
                            },
                            new DbItem("Email List Separator", ID.NewID, TemplateIDs.DictionaryEntry)
                            {
                                {"Key","Forms.Actions.SendMail.EmailListSeparator" },
                                {"Phrase","," }
                            },
                            new DbItem("Keyword Prefix", ID.NewID, TemplateIDs.DictionaryEntry)
                            {
                                {"Key","Forms.Actions.SendMail.KeywordPrefix" },
                                {"Phrase","{" }
                            },
                            new DbItem("Keyword Suffix", ID.NewID, TemplateIDs.DictionaryEntry)
                            {
                                {"Key","Forms.Actions.SendMail.KeywordSuffix" },
                                {"Phrase","}" }
                            },
                            new DbItem("List Separator", ID.NewID, TemplateIDs.DictionaryEntry)
                            {
                                {"Key","Forms.Actions.SendMail.ListSeparator" },
                                {"Phrase",", " }
                            },
                            new DbItem("Unsupported field type Error Message", ID.NewID, TemplateIDs.DictionaryEntry)
                            {
                                {"Key","Forms.Actions.SendMail.UnsupportedFieldTypeErrorMessage" },
                                {"Phrase","Unsupported field type {0}" }
                            }
                        }
                     }
                }
            };

            dictionary.ParentID = ItemIDs.SystemRoot;
            return dictionary;

        }

        private List<IViewModel> BuildFormSubmitContextFields()
        {
            var fields = new List<FieldViewModel>();

            fields.Add(new StringInputViewModel { Name = "From", Title = "From", Value = "from@domain.com", ItemId = ID.NewID.ToString() });
            fields.Add(new StringInputViewModel { Name = "To", Title = "To", Value = "to@domain.com", ItemId = ID.NewID.ToString() });
            fields.Add(new StringInputViewModel { Name = "Cc", Title = "Cc", Value = "cc@domain.com", ItemId = ID.NewID.ToString() });
            fields.Add(new StringInputViewModel { Name = "Bcc", Title = "Bcc", Value = "bcc@domain.com", ItemId = ID.NewID.ToString() });
            fields.Add(new StringInputViewModel { Name = "Subject", Title = "Subject", Value = "Subject very important for a test", ItemId = ID.NewID.ToString() });
            fields.Add(new StringInputViewModel { Name = "MessageText", Title = "MessageText", Value = "Message text for a very important email", ItemId = ID.NewID.ToString() });

            return fields.ToList<IViewModel>();
        }

        public SendMailActionTests(ISubmitActionData submitActionData) : base(submitActionData)
        {
        }

        public SendMailActionTests() : base(Substitute.For<ISubmitActionData>())
        {

        }

        [Test]
        public void Execute_SendMailActionDataIsNull_ReturnsFalse()
        {
            // Arrange
            var sendMailActionTests = new SendMailActionTests();
            // Act
            var ret = sendMailActionTests.Execute(null, Substitute.ForPartsOf<FormSubmitContext>(ID.NewID.ToString()));
            // Assert
            Assert.That(ret, Is.False);

        }

        [Test]
        public void Execute_ReferenceIdIsNull_ReturnsFalse()
        {
            // Arrange
            var sendMailActionTests = new SendMailActionTests();
            // Act
            var sendMailActionData = new SendMailActionData();
            var ret = sendMailActionTests.Execute(sendMailActionData, Substitute.ForPartsOf<FormSubmitContext>(ID.NewID.ToString()));
            // Assert
            Assert.That(ret, Is.False);

        }

        [Test]
        public void Execute_EmailTemplateIsInvalid_ReturnsFalse()
        {

            using (var db = new Db { })
            {
                // Arrange
                var sendMailActionTests = new SendMailActionTests();
                // Act
                var sendMailActionData = new SendMailActionData() { ReferenceId = ID.NewID.ToGuid() };
                var ret = sendMailActionTests.Execute(sendMailActionData, Substitute.ForPartsOf<FormSubmitContext>(ID.NewID.ToString()));
                // Assert
                Assert.That(ret, Is.False);
            }
        }

        [Test]
        public void FillMailAddressCollection_ValidEmails_FillMailAddressCollection()
        {

            // Arrange
            MailMessage mailMessage = new MailMessage();
            string[] mailAddresses = new[] { "email1@domain.com", "email2@domain.com", "email3@domain.com", "email4@domain.com" };
            var sendMailActionTests = new SendMailActionTests();
            // Act
            sendMailActionTests.FillMailAddressCollection(mailAddresses, mailMessage.To);
            // Assert
            Assert.That(mailMessage.To, Has.Count.EqualTo(4));
        }

        [Test]
        public void FillMailAddressCollection_ZeroEmails_FillMailAddressCollection()
        {

            // Arrange
            MailMessage mailMessage = new MailMessage();
            string[] mailAddresses = "".Split(',');
            var sendMailActionTests = new SendMailActionTests();
            // Act
            sendMailActionTests.FillMailAddressCollection(mailAddresses, mailMessage.To);
            // Assert
            Assert.That(mailMessage.To, Has.Count.Zero);
        }

        [Test]
        public void SplitEmails_FiveEmails_ReturnsFiveEmails()
        {

            using (var db = new Db { BuildDictionaries() })
            {
                // Arrange
                MailMessage mailMessage = new MailMessage();
                string mailstring = "email1@domain.com, email2@domain.com, email3@domain.com, email4@domain.com, email5@domain.com";
                var sendMailActionTests = new SendMailActionTests();
                // Act
                var emails = sendMailActionTests.SplitEmails(mailstring);
                // Assert
                Assert.That(emails.Length, Is.EqualTo(5));
            }
        }

        [Test]
        public void SplitEmails_ZeroEmails_ReturnsZeroEmails()
        {

            using (var db = new Db { BuildDictionaries() })
            {
                // Arrange
                MailMessage mailMessage = new MailMessage();
                string mailstring = "";
                var sendMailActionTests = new SendMailActionTests();
                // Act
                var emails = sendMailActionTests.SplitEmails(mailstring);
                // Assert
                Assert.That(emails.Length, Is.Zero);
            }
        }


    }
}
