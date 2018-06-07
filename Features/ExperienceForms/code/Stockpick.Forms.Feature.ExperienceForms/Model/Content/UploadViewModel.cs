using System;
using Sitecore;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Mvc.Models.Fields;

namespace Stockpick.Forms.Feature.ExperienceForms.Model.Content
{
    public class UploadViewModel : InputViewModel<string>
    {
        protected override void InitItemProperties(Item item)
        {
            Assert.ArgumentNotNull((object)item, nameof(item));
            base.InitItemProperties(item);
        }

        protected override void UpdateItemFields(Item item)
        {
            Assert.ArgumentNotNull((object)item, nameof(item));
            base.UpdateItemFields(item);
        }
    }
}