namespace MvcTables
{
    #region

    using System;

    #endregion

    internal static class DelegateHelperExtensions
    {
        public static void ExecuteIfNotNull<TType>(this Action<TType> action, TType param)
        {
            if (action != null)
            {
                action(param);
            }
        }

        public static TResult ExecuteIfNotNull<TType, TResult>(this Func<TType, TResult> func, TType param)
        {
            if (func != null)
            {
                return func(param);
            }
            return default(TResult);
        }

        public static TResult ExecuteIfNotNull<TType1, TType2, TResult>(this Func<TType1, TType2, TResult> func,
                                                                        TType1 param1, TType2 param2)
        {
            if (func != null)
            {
                return func(param1, param2);
            }
            return default(TResult);
        }
    }
}