using Sitecore.Data;

namespace Stockpick.Forms.Feature.ExperienceForms
{
    public struct Constants
    {

        public struct Dictionary
        {
            public struct Forms
            {
                public struct Actions
                {
                    public struct SendMail
                    {
                        public const string EmailListSeparator = "Forms.Actions.SendMail.EmailListSeparator";
                        public const string ListSeparator = "Forms.Actions.SendMail.ListSeparator";
                        public const string CheckboxCheckedText = "Forms.Actions.SendMail.CheckboxCheckedText";
                        public const string CheckboxUncheckedText = "Forms.Actions.SendMail.CheckboxUncheckedText";
                        public const string DateFormatMask = "Forms.Actions.SendMail.DateFormatMask";
                        public const string DoubleFormatMask = "Forms.Actions.SendMail.DoubleFormatMask";
                        public const string UnsupportedFieldTypeErrorMessage = "Forms.Actions.SendMail.UnsupportedFieldTypeErrorMessage";
                        public const string KeywordPrefix = "Forms.Actions.SendMail.KeywordPrefix";
                        public const string KeywordSuffix = "Forms.Actions.SendMail.KeywordSuffix";
                    }
                }
            }
        }

        public struct Templates
        {
            public struct Content
            {
                public struct Hidden
                {
                    public struct Fields
                    {
                        public static readonly ID Value = new ID("{29D58F06-3CA7-443A-A2CF-9A2916FC280B}");
                    }
                }
            }


        }
    }
}