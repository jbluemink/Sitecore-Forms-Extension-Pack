using System.Collections.Generic;
using System.Reflection;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using Stockpick.Forms.Feature.ExperienceForms.Model;

namespace Stockpick.Forms.Feature.ExperienceForms.Submit
{
    /// <summary>
    /// </summary>
    /// <seealso cref="Sitecore.ExperienceForms.Processing.Actions.SubmitActionBase{TParametersData}" />
    public class AzureQueueSubmit : SubmitActionBase<string>
    {
        private string _connectionstring;
        /// <summary>
        /// Initializes a new instance of the <see cref="LogSubmit"/> class.
        /// </summary>
        /// <param name="submitActionData">The submit action data.</param>
        public AzureQueueSubmit(ISubmitActionData submitActionData) : base(submitActionData)
        {
            var stockpickFormsAzurequeueConnectionstring = "Stockpick.Forms.AzureQueue.Connectionstring";
            _connectionstring = Sitecore.Configuration.Settings.GetSetting(stockpickFormsAzurequeueConnectionstring);
            if (string.IsNullOrEmpty(_connectionstring))
            {
                Log.Warn("AzureQueueSubmit Forms configuration setting missing "+ stockpickFormsAzurequeueConnectionstring,this);
            }
        }

        /// <summary>
        /// Tries to convert the specified <paramref name="value" /> to an instance of the specified target type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="target">The target object.</param>
        /// <returns>
        /// true if <paramref name="value" /> was converted successfully; otherwise, false.
        /// </returns>
        protected override bool TryParse(string value, out string target)
        {
            target = string.Empty;
            return true;
        }

        /// <summary>
        /// Executes the action with the specified <paramref name="data" />.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="formSubmitContext">The form submit context.</param>
        /// <returns>
        ///   <c>true</c> if the action is executed correctly; otherwise <c>false</c>
        /// </returns>
        protected override bool Execute(string data, FormSubmitContext formSubmitContext)
        {
            Assert.ArgumentNotNull(formSubmitContext, nameof(formSubmitContext));
            Assert.ArgumentNotNullOrEmpty(_connectionstring, nameof(_connectionstring));

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_connectionstring);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a container. use only lowercase!!
            CloudQueue queue = queueClient.GetQueueReference("stockpickformsqueue");

            // Create the queue if it doesn't already exist
            queue.CreateIfNotExists();

            // Create a message 
            var message = new FormFields
            {
                FormId = formSubmitContext.FormId.ToString(),
                Fields = new List<FormFieldSmall>()
            };

            foreach (var viewModel in formSubmitContext.Fields)
            {
                var postedField = (IValueField) viewModel;
                IValueField valueField = postedField as IValueField;
                PropertyInfo property = postedField.GetType().GetProperty("Value");
                object postedValue =
                    (object) property != null ? property.GetValue((object) postedField) : (object) null;
                property = postedField.GetType().GetProperty("Title");
                object postedTitle =
                    (object) property != null ? property.GetValue((object) postedField) : (object) null;
                if (valueField.AllowSave && postedValue != null && postedTitle != null)
                {
                    message.Fields.Add(new FormFieldSmall()
                    {
                        Name = viewModel.Name,
                        Title = postedTitle.ToString(),
                        ItemId = viewModel.ItemId,
                        Value = postedValue.ToString()
                    });
                }
            }

            // Create a queue message with JSON and add it to the queue.
            CloudQueueMessage queuemessage = new CloudQueueMessage(JsonConvert.SerializeObject(message));
            queue.AddMessage(queuemessage);
            return true;
        }


       
    }
}

