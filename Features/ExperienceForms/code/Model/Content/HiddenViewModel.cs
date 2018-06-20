using Sitecore;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Mvc.Models.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stockpick.Forms.Feature.ExperienceForms.Model.Content
{
    public class HiddenViewModel : StringInputViewModel
    {

        public string HiddenValue { get; set; }

        public HiddenViewModel()
        {
        }

        protected override void InitItemProperties(Item item)
        {

            Assert.ArgumentNotNull(item, nameof(item));
            base.InitItemProperties(item);

            Field field = item.Fields[Constants.Templates.Content.Hidden.Fields.HiddenValue];

            string value = null;
            if (field != null)
                value = field.Value;

            this.HiddenValue = StringUtil.GetString(value);

        }

        protected override void UpdateItemFields(Item item)
        {
            base.UpdateItemFields(item);
            item.Fields[Constants.Templates.Content.Hidden.Fields.HiddenValue]?.SetValue(HiddenValue, true);
        }

    }
}