namespace MvcTables.Render
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    #endregion

    internal static class RouteValueDictionaryExtensions
    {
        public static TDict WithClass<TDict>(this TDict dict, string filter)
            where TDict : IDictionary<string, object>
        {
            if (!dict.ContainsKey("class"))
            {
                dict["class"] = "";
            }
            dict["class"] = dict["class"] + " " + filter;
            return dict;
        }

        public static TDict WithStyle<TDict>(this TDict dict, string key, string value)
            where TDict : IDictionary<string, object>
        {
            if (!dict.ContainsKey("style"))
            {
                dict["style"] = String.Format(@"{0}:{1};", key, value);
                return dict;
            }
            else
            {
                var style = dict["style"].ToString();
                if (!style.Contains(key))
                {
                    style += String.Format("{0}:{1};", key, value);
                    dict["style"] = style;
                }
                else
                {
                    var styleRegex = new Regex(String.Format(@"(?<={0}:)[^;\s]+", key));
                    styleRegex.Replace(style, value);
                }
            }

            return dict;
        }
    }
}