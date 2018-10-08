using Sitecore;
using Sitecore.Data.Items;
using Sitecore.ExperienceForms.Mvc.Models.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stockpick.Forms.Feature.ExperienceForms.Model.Content
{
    public class HiddenViewModel : FieldViewModel
    {

        public string Value { get; set; }

        public HiddenViewModel()
        {
        }

        protected override void InitItemProperties(Item item)
        {
            base.InitItemProperties(item);
            this.Value = StringUtil.GetString(item.Fields[Constants.Templates.Content.Hidden.Fields.Value]);
        }

        protected override void UpdateItemFields(Item item)
        {
            base.UpdateItemFields(item);
            item.Fields[Constants.Templates.Content.Hidden.Fields.Value]?.SetValue(Value, true);
        }

    }
}