namespace MvcTables
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Web;

    #endregion

    public static class NameValueCollectionExtensions
    {
        public static string ToQueryString<TDict>(this TDict qs)
            where TDict : IDictionary<string, object>
        {
            return string.Join("&",
                               Array.ConvertAll(
                                                qs.Keys.Where(k => !String.IsNullOrEmpty(k) && qs[k] != null).ToArray(),
                                                key =>
                                                string.Format("{0}={1}", HttpUtility.UrlEncode(key),
                                                              HttpUtility.UrlEncode(qs[key].ToString()))));
        }

        public static IDictionary<string, object> Merge<TDict>(this TDict values, TDict mergeWith)
            where TDict : IDictionary<string, object>
        {
            foreach (var kvp in mergeWith.Where(kvp => !values.ContainsKey(kvp.Key)))
            {
                values[kvp.Key] = kvp.Value;
            }

            return values;
        }

        public static NameValueCollection Merge(this NameValueCollection source, object obj)
        {
            if (obj == null)
            {
                return source;
            }
            var props = obj.GetType().GetProperties().Where(p => p.CanRead);
            foreach (var prop in props)
            {
                var val = prop.GetValue(obj);
                if (val != null)
                {
                    source[prop.Name] = prop.GetValue(obj).ToString();
                }
            }
            return source;
        }

        public static string ToQueryString(this NameValueCollection nvc)
        {
            var array = (from key in nvc.AllKeys
                         from value in nvc.GetValues(key)
                         select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value)))
                .ToArray();
            return string.Join("&", array);
        }
    }
}