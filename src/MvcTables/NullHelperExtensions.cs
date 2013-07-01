namespace MvcTables
{
    #region

    using System;

    #endregion

    internal static class NullHelperExtensions
    {
        public static TResult IfIsNull<TTarget, TResult>(this TTarget target, Func<TTarget, TResult> getter)
            where TTarget : class
        {
            if (target == null)
            {
                return default(TResult);
            }

            return getter(target);
        }
    }
}