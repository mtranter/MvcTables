namespace MvcTables
{
    #region

    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    #endregion

    internal class ReflectedColumn<TModel, TColumn> : ReflectedColumnBase<TModel, TColumn>
    {
        public ReflectedColumn(Expression<Func<TModel, TColumn>> columnDefinition) : base(columnDefinition)
        {
        }

        protected override MvcHtmlString GetCellValueCore(TModel[] model, int rowIndex, TColumn column,
                                                          HtmlHelper<TModel[]> helper)
        {
            var sRetval = FormatValue(column);
            return MvcHtmlString.Create(sRetval);
        }
    }
}