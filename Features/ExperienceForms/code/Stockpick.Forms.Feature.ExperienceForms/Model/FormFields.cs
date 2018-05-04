using System.Collections.Generic;

namespace Stockpick.Forms.Feature.ExperienceForms.Model
{
    public class FormFields
    {
        public string FormId { set; get; }
        public IList<FormFieldSmall> Fields { set; get; }
    }
}