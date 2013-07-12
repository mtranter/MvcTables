namespace MvcTables
{
    #region

    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Routing;

    #endregion

    internal static class DictionaryExtensions
    {
        public static IDictionary<TKey, TValue> Merge<TKey, TValue, TDict>(this TDict source,
                                                                           TDict other)
            where TDict : IDictionary<TKey, TValue>
        {
            return source.Union(other).ToDictionary(k => k.Key, v => v.Value);
        }

        public static IDictionary<string, object> Merge(this IDictionary<string, object> source, object other)
        {
            return source.Union(new RouteValueDictionary(other)).ToDictionary(k => k.Key, v => v.Value);
        }

        public static IDictionary<string, object> Merge(this IDictionary<string, object> source,
                                                        IDictionary<string, object> other)
        {
            return source.Union(new RouteValueDictionary(other)).ToDictionary(k => k.Key, v => v.Value);
        }

        public static IDictionary<TKey, TValue> Clone<TKey, TValue>(this IDictionary<TKey, TValue> source)
        {
            return new Dictionary<TKey, TValue>(source);
        }
    }
}