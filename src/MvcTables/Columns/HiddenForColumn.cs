namespace MvcTables
{
    #region

    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    #endregion

    internal class HiddenForColumn<TModel, TColumn> : ReflectedColumnBase<TModel, TColumn>
    {
        public HiddenForColumn(Expression<Func<TModel, TColumn>> columnDefinition)
            : base(columnDefinition)
        {
        }

        protected override MvcHtmlString GetCellValueCore(TModel[] model, int rowIndex, TColumn cellValue,
                                                          HtmlHelper<TModel[]> helper)
        {
            return helper.HiddenFor(ArrayIndexExpression(model, rowIndex));
        }
    }
}