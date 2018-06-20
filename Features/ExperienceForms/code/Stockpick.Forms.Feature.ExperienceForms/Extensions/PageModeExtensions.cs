using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Helpers;

namespace Stockpick.Forms.Feature.ExperienceForms.Extensions
{
    public static class HtmlHelperExtensions
    {

        private const string FormsModeQueryStringParameter = "sc_formmode";

        public static bool IsExperienceForms(this SitecoreHelper helper)
        {
            return Sitecore.Context.Request.QueryString[FormsModeQueryStringParameter] != null;
        }

    }
}