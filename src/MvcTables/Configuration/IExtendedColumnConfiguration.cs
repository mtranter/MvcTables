namespace MvcTables
{
    #region

    using System;

    #endregion

    public interface IFormatableColumnConfiguration<TModel, out TColumn> : IColumnConfiguration<TModel>
    {
        IColumnConfiguration<TModel> SetFormatString(string format);
        IColumnConfiguration<TModel> FormatUsing(Func<TColumn, string> formatUsing);
    }
}